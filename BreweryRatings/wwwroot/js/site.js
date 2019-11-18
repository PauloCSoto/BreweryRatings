// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$.fn.stars = function () {
return this.each(function (_i, e) {$(e).html($('<span/>').width($(e).text() * 16)); });
};

$('.stars').stars();