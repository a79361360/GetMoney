// JavaScript Document
$(document).ready(function () {
    //ios();
    zanwei();
    $(".choice li").click(function () {
        var val1 = $(this).children("u").text();
        var val2 = val1.substring(0, val1.indexOf("元"));
        //var fw_numb = $(".srkk").val();
        //var fw_tal = fw_price * fw_numb;
        $(".srkk").val(val2);
        //console.log($(".srkk").val())
        $(this).addClass("on").siblings().removeClass("on");
    });
});
$(window).resize(function () {
    //ios();
	zanwei();
})
function ios() {
    var u = navigator.userAgent;
    var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
    if (isiOS) {
        $(".choice li:eq(3)").css("display", "none");
        $(".choice li:eq(4)").css("display", "none");
        $(".choice li:eq(5)").css("display", "none");
    } else {
        $(".choice li:eq(3)").css("display", "block");
        $(".choice li:eq(4)").css("display", "block");
        $(".choice li:eq(5)").css("display", "block");
    }
}
function zanwei() {
	var dh = window.innerHeight;
	var wh =$(".wrap").height();
	if(wh < dh){
		var h = dh - wh;
		$(".zanwei").css("height",h)
	}
}
