
window.onload = function () { document.getElementById("loading").style.display="none" }

$('.nav-toggle').click(function (e) {

    e.preventDefault();
    $("html").toggleClass("openNav");
    $(".nav-toggle").toggleClass("active");

});








