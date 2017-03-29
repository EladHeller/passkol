$(document).ready(function() {

    Date.prototype.addDays = function (num) {
        var value = this.valueOf();
        value += 86400000 * num;
        return new Date(value);
    }

    Date.prototype.addSeconds = function (num) {
        var value = this.valueOf();
        value += 1000 * num;
        return new Date(value);
    }

    Date.prototype.addMinutes = function (num) {
        var value = this.valueOf();
        value += 60000 * num;
        return new Date(value);
    }

    Date.prototype.addHours = function (num) {
        var value = this.valueOf();
        value += 3600000 * num;
        return new Date(value);
    }

    Date.prototype.addMonths = function (num) {
        var value = new Date(this.valueOf());

        var mo = this.getMonth();
        var yr = this.getYear();

        mo = (mo + num) % 12;
        if (0 > mo) {
            yr += (this.getMonth() + num - mo - 12) / 12;
            mo += 12;
        }
        else
            yr += ((this.getMonth() + num - mo) / 12);

        value.setMonth(mo);
        value.setYear(yr);
        return value;
    }

	function nomobile() {
		if ($(window).width() < 768)
		    window.location.href = "/HtmlTemplates/nomobile";
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
		    span.removeClass("faqopen fa-chevron-up").addClass("faqclose fa-chevron-down");
		}
		else {
		    span.removeClass("faqclose fa-chevron-down").addClass("faqopen fa-chevron-up");
		}
		$(this).next().toggle("");
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
		    $(".account .fa").removeClass("fa-caret-down").addClass("fa-caret-up");
			$(".accountmenu1").show();
		},
		function() {
			$(".accountmenu1").hide();
			$(".account .fa").removeClass("fa-caret-up").addClass("fa-caret-down");
		}
	);

	$("[rel='tooltip']").tooltip();
	
	$('[data-toggle="popover"]').popover()
	
});