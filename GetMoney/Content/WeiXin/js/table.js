// JavaScript Document
$(document).ready(function () {

	//伸缩
	$("#key").click(function() {
		$(this).html($(".screen").is(":hidden") ? "收起" : "展开");
		$(".screen").slideToggle();
	});
	
	//日期设置
	var myDate = new Date();
	var yy = myDate.getFullYear();
	var mm = myDate.getMonth()+1;
	var dd = myDate.getDate();
	var maxDate = yy + '-' + mm + '-' + dd;
	var date = new Date(myDate.getTime() - 365 * 24 * 3600 * 1000);
	var y7 = date.getFullYear();
	var m7 = date.getMonth()+1;
	var d7 = date.getDate();
	//console.log(yy+'-'+mm+'-'+dd);
	var day3 = new Date();
	day3.setTime(day3.getTime()+24*60*60*1000);
	var s3 = day3.getFullYear()+"," + (day3.getMonth()+1) + "," + day3.getDate();
	
	var options = {
		dateInputNode : $(".my-input"),
		selectionRule: {
			single: false,
			range: true,
			rangeDisableSelect: true,
		},
		modules: {
			today: false,
			gotoDate: false,
			clear: false
		},
		disablingRules: [{from :[s3] , to:'>'}]
	}
	var instance = new BeatPicker(options);
	var from = y7 + '-' + m7 + '-' + d7;
	var to = maxDate;
	$(".beatpicker-input-from").val(from);
	$(".beatpicker-input-to").val(to);
	
	$('.goTop').on('click',function(){
		//var container = $('#MyTable_tableData');container.scrollTop(0);//滚动到div 100px
		$('#MyTable_tableData').animate({scrollTop: 0}, 300);
		return false;
	});
	
	//底部菜单
	$(".footnav li").click(function() {
		$(this).siblings().children('.footdiv').hide();
		if($(this).children('.footdiv').is(':hidden')){
			$(this).children('.footdiv').show()
		}else{
			$(this).children('.footdiv').hide()
		}
	}); 
});
/*window.onload = function(){
	//768代表设计师给的设计稿的宽度，你的设计稿是多少，就写多少;100代表换算比例，这里写100是为了以后好算,比如，你测量的一个宽度是100px,就可以写为1rem,以及1px=0.01rem等等
	getRem(768,100)
};
window.onresize = function(){
	getRem(768,100)
};*/
function getRem(pwidth,prem){
	call(pwidth,prem);
}

function call(pwidth,prem){
	var html = document.getElementsByTagName("html")[0];
	var oWidth = document.body.clientWidth || document.documentElement.clientWidth;
	var oHeight = document.body.clientHeight || document.documentElement.clientHeight;
	if (oWidth >= pwidth) {
		oWidth = pwidth;
	}if (oWidth > oHeight) {
		oWidth = oHeight
	}
	var size=oWidth/pwidth*prem
	html.style.fontSize = size + "px";
	//以下为表格特效代码
	var top=1.6*size+30+0.1*size;
	var h=oHeight-top-0.9*size
	$('.cont').css({"top": top,"height":h});
	
	var w = $(".data-table").width() - 1;
	$('.data-list').css("width", w);
	ss = $(".data-list").width();
	if(/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
		FixTable("MyTable", 1, "100%", h);
	} else {
		FixTable("MyTable", 1, ss, h);
	}
}
function call2(pwidth, prem) {
    var html = document.getElementsByTagName("html")[0];
    var oWidth = document.body.clientWidth || document.documentElement.clientWidth;
    var oHeight = document.body.clientHeight || document.documentElement.clientHeight;
    if (oWidth >= pwidth) {
        oWidth = pwidth;
    } if (oWidth > oHeight) {
        oWidth = oHeight
    }
    var size = oWidth / pwidth * prem
    html.style.fontSize = size + "px";
    //以下为表格特效代码
    var conditional = 0.1 * size;
    $('.conditional').css({ "top": conditional });
    var top = 30 + 0.2 * size;
    var h = oHeight - top - 0.9 * size;
    $('.cont').css({ "top": top, "height": h });
    var w = $(".data-table").width() - 1;
    $('.data-list').css("width", w);
    ss = $(".data-list").width();
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
        FixTable("MyTable", 1, "100%", h);
    } else {
        FixTable("MyTable", 1, ss, h);
    }
}