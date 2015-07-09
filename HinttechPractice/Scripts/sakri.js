$(document).ready(function () {
    $("#bilokako").show();
    $("#spanIDhide").hide();
    $("#spanIDshow").show();
        $("#spanIDshow").click(function () {
            $("#bilokako").hide();
            $("#spanIDshow").hide();
            $("#spanIDhide").show();
        });
        $("#spanIDhide").click(function () {
            $("#bilokako").show();
            $("#spanIDhide").hide();
            $("#spanIDshow").show();
        });
    });


