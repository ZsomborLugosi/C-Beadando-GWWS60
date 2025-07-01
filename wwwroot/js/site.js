// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function() {
    // Auto-hide success messages
    const successMessage = $('.success-message');
    if (successMessage.length) {
        setTimeout(function() {
            successMessage.fadeOut('slow');
        }, 5000); // 5 seconds
    }

    // Auto-focus on the first visible input in forms whose action ends with /Create or /Edit
    // This is a common pattern for create/edit pages.
    const formFirstInput = $('form[action$="/Create"] input:visible:first, form[action$="/Edit"] input:visible:first');
    if (formFirstInput.length) {
        formFirstInput.focus();
    }
});
