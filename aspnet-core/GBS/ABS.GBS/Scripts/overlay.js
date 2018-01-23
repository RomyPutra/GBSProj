var $overlay_wrapper;
var $overlay_panel;

function show_overlay() {
    if (!$overlay_wrapper) append_overlay();
    $overlay_wrapper.fadeIn(700);
}

function hide_overlay() {
    $overlay_wrapper.fadeOut(500);
}

function append_overlay() {
    $overlay_wrapper = $('<div id="overlay"></div>').appendTo($('BODY'));
    $overlay_panel = $('<div id="overlay-panel"><img src="../Images/Airasia/loading_circle.gif" /></div>').appendTo($overlay_wrapper);
}

