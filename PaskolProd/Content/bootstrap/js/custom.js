$(document).ready(function() {

	function nomobile() {
		if ($(window).width() < 768)
			window.location.href = "nomobile.html";
	}

	nomobile();
	$(window).resize(function () {
		nomobile();
	});
	
	$("#searches ul").jScrollPane();
	
	$(".selector span, .selector i").click(function() {
		$(this).parent().find("ul").toggle();
	});
	
	$(".selector ul li a").click(function() {
		var p = $(this).closest(".selector");
		var t = $(this).text();
		p.find("span").text(t);
		p.find("ul").hide();
		return false;
	});
	
	$(".faqheading").click(function() {
		var span = $(this).find("span:last-child");
		if (span.hasClass("faqopen")) {
			span.removeClass("faqopen glyphicon-menu-up").addClass("faqclose glyphicon-menu-down");
		}
		else {
			span.removeClass("faqclose glyphicon-menu-down").addClass("faqopen glyphicon-menu-up");
		}
		$(this).next().slideToggle();
	});
	
	$(".closeall").click(function() {
		$(".collapseme").hide();
		$(".faqclose").removeClass("faqclose glyphicon-menu-down").addClass("faqopen glyphicon-menu-up");
		return false;
	});
	
	$("#togglemanager").click(function() {
		$(this).toggleClass("toggled");
		$(this).next().toggle();
		return false;
	});
	
	$(".fileinput input").change(function() {
		var arr = $(this).val().split("\\");
		var file = arr[arr.length-1];
		$(this).parent().find("span:first-child").text(file);
	});
	
	$(".account").hover(
		function() {
			$(".account > span").removeClass("glyphicon-triangle-bottom").addClass("glyphicon-triangle-top");
			$(".accountmenu1").show();
		},
		function() {
			$(".accountmenu1").hide();
			$(".account > span").removeClass("glyphicon-triangle-top").addClass("glyphicon-triangle-bottom");
		}
	);

	$("[rel='tooltip']").tooltip();
	
	$('[data-toggle="popover"]').popover()
	
});