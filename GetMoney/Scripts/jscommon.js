$(function () {
    var clock = new Clock();
    clock.display($('#clock'));
    InitLeftMenu();
});


//时间日期
function Clock() {
    var date = new Date();
    this.year = date.getFullYear();
    this.month = date.getMonth() + 1;
    this.date = date.getDate();
    this.day = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六")[date.getDay()];
    this.hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
    this.minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
    this.second = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

    this.toString = function () {
        return this.year + "年" + this.month + "月" + this.date + "日 " + this.hour + ":" + this.minute + ":" + this.second + " " + this.day;
    };

    this.toSimpleDate = function () {
        return this.year + "-" + this.month + "-" + this.date;
    };

    this.toDetailDate = function () {
        return this.year + "-" + this.month + "-" + this.date + " " + this.hour + ":" + this.minute + ":" + this.second;
    };

    this.display = function (ele) {
        var clock = new Clock();
        ele.html(clock.toString());
        window.setTimeout(function () { clock.display(ele); }, 1000);
    };
}
function InitLeftMenu() {
    $('.layout-split-west .easyui-accordion .panel .accordion-body ul li').click(function () {
        var url = "Card/Test";
        var title = $(this).find('.tree-title').text();
        OpenTabs(url, title, 'icon-ok');
    });
}
function OpenTabs(url, title, icon) {
    //console.log(url);
    //console.log(title);
    //console.log(icon);

    var tt = $('#tabsContainer');
    if (!tt.tabs('exists', title)) {
        $.getServerView(url, null, function (res) {
            //console.log(url);
            console.log(res); 
            tt.tabs('add', {
                title: title,
                closable: true,
                border: false,
                content: res,
                iconCls: icon
            });
        });
    } else {
        tt.tabs('select', title)
    }
}

$.fn.extend({
    getJsonValues: function () {
        var jsones = '{';
        var texts = this.find("input[type=text],input[type=radio][checked=true],input[type=checkbox][checked=true]");
        texts.each(function () {
            jsones += $(this).attr("name") + ':"' + $(this).val() + '",';
        });
        if (jsones.length > 0) { jsones = jsones.substr(0, jsones.length - 1) + '}'; }
        this.pageValues = jsones;
        return this;
    },
    getUrlValues: function () {
        var urlData = '';
        var texts = this.find("input[type=text],input[type=radio][checked=true],input[type=checkbox][checked=true],input[type=hidden],select,textarea");
        texts.each(function () {
            urlData += $(this).attr("name") + '=' + $(this).val() + '&';
        });
        if (urlData.length > 0) { urlData = urlData.substr(0, urlData.length - 1); }
        this.pageValues = urlData;
        return this;
    },
    setValues: function (res) {
        this.pageValues = res;
        return this;
    },
    requestType: function (reqType) {
        this.reqType = reqType;
        return this;
    },
    callBackFunction: function (res) {
        this.callBackFunction = res;
        return this;
    },
    setRequestHeaderType: function (res) {
        this.requestHeaderType = res;
        return this;
    },
    doAjax: function (url) {
        var me = this;
        if (me.validate() == false) {
            return false;
        }
        var ajaxType = typeof (this.reqType) === "undefined" ? "GET" : this.reqType.toLocaleUpperCase();
        var actualValue = me.pageValues;
        if (ajaxType !== "GET") {
            if (actualValue.lastIndexOf("=") == actualValue.indexOf("=")) {
                actualValue = actualValue.substring(actualValue.indexOf("="));
            }
        }

        $.ajax({
            cache: false,
            headers: {
                "Content-Type": typeof (this.requestHeaderType) === "undefined" ? "application/x-www-form-urlencoded" : this.requestHeaderType
            },
            beforeSend: function (XMLHttpRequest) {
                XMLHttpRequest.setRequestHeader("Syscuruserkey", $.syscuruserkey);
            },
            type: ajaxType,
            url: $.rootURL + url + (ajaxType === "GET" ? "?" + actualValue : ""),
            data: ajaxType === "GET" ? "{}" : actualValue,
            dataType: 'json',
            success: function (result) {
                if (me.callBackFunction) {
                    me.callBackFunction(result);
                }
            },
            error: function (result, status) {
                if (status === 'error') {
                    me.callBackFunction(result);
                }
            }
        });
        return this;
    },
    validate: function () {
        if (this.is("form") === true) {
            return this.form('validate');
        }
        else {
            var isValid = true;
            $(":input", this).each(function () {
                var input = $(this);
                if (input.attr("required") == null && input.attr("data-options") == null) {
                    return;
                }
                if (!input.validatebox("isValid")) {
                    isValid = false;
                    input.focus();
                }
            });
            return isValid;
        }
    }
});
//-----------------------------------------------------
//extend the static method for the ajax request.
//-----------------------------------------------------
(function ($) {
    $.extend({
        getServerView: function (method, parameter, callbackFun) {
            $.nemoLayout.ShowLoading();
            console.log(method)
            $.ajax({
                cache: false,
                type: "Get",
                contentType: "application/json; charset=utf-8",
                url: $.rootURL + method,
                data: parameter == null ? "{}" : parameter,
                dataType: 'html',
                success: function (result) {
                    if (callbackFun) {
                        callbackFun(result);
                    }
                    $.nemoLayout.CloseLoading();
                },
                error: function (result, status) {
                    if (status == 'error') {
                        callbackFun(result);
                    }
                    $.nemoLayout.CloseLoading();
                }
            });
        },
        getServerJson: function (method, parameter, callbackFun) {
            $.nemoLayout.ShowLoading();
            $.ajax({
                cache: false,
                type: "Get",
                contentType: "application/json; charset=utf-8",
                url: $.rootURL + method,
                data: parameter == null ? "{}" : parameter,
                dataType: 'json',
                success: function (result) {
                    if (callbackFun) {
                        callbackFun(result);
                    }
                    $.nemoLayout.CloseLoading();
                },
                error: function (result, status) {
                    if (status == 'error') {
                        callbackFun(result);
                    }
                    $.nemoLayout.CloseLoading();
                }
            });
        },
        getQueryString: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]);
            return null;
        },
        rootURL: "/",
        pageSize: 10,
        pageList: [10, 20, 50, 100],
    });
})(jQuery);


(function ($) {
    $.nemoLayout = {
        ShowLoading: function () {
            $('#showloadmain').css({ height: $(window).height(), width: $(window).width() }).show();
            $('#showloading').css({
                top: ($(window).height() - 60) * 0.5,
                left: ($(window).width() - 185) * 0.5,
            }).show();
            $('#showloadmain').click(function () {
                $('#showloading,#showloadmain').hide();
            });
        },
        CloseLoading: function () {
            $('#showloading,#showloadmain').hide();
        }
    };
})(jQuery);