this.checkboxGroupAnimation = function(el) {
  var temp = true;
  el.on("change", function() {
    if (temp) {
      var checkboxBar = el
          .closest(".checkbox-group")
          .find(".checkbox-group-bar"),
        dur = checkboxBar.css("transition-duration");
      temp = false;
      dur = dur.slice(0, -1);
      dur = +dur * 1000;
      checkboxBar.addClass("on");
      setTimeout(function() {
        checkboxBar.removeClass("on");
        temp = true;
      }, dur);
    }
  });
};

this.createCheckboxLabel = function(el) {
  $(el)
    .find("input.checkbox")
    .each(function() {
      if (!$(this).is(":visible")) {
        return;
      }
      var checboxId = "checkbox" + self.generateId();
      $(this)
        .addClass("checkbox")
        .attr("id", checboxId)
        .after('<label for = "' + checboxId + '"></label>');
    });

  $(el)
    .find("input.check-box")
    .each(function() {
      if (!$(this).is(":visible")) {
        return;
      }
      var checboxId = "checkbox" + self.generateId();
      $(this)
        .addClass("checkbox")
        .attr("id", checboxId)
        .after('<label for = "' + checboxId + '"></label>');
    });
};

// if($('input').is('.checkbox')){
//     $('.checkbox').each(function(){
//         self.checkboxGroupAnimation($(this));
//     })
// }

if ($("input").is(".check-box")) {
  $(".check-box").each(function() {
    var checboxId = "checkbox" + self.generateId();
    $(this)
      .addClass("checkbox")
      .attr("id", checboxId)
      .after('<label for = "' + checboxId + '"></label>');
  });
}
