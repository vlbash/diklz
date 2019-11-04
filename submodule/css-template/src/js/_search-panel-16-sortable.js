this.createSortablePresetFilters = function (formObj) {

     // $(formObj.presetWrapper).sortable({
     //      cursor: "move",
     //      axis: "y",
     //      items: "li:not(.content-search-form-preset-header)",
     //      start: startSortable,
     //      stop: stopSorting
     // }).disableSelection();


     // function startSortable(e, ui) {
     //      $(ui.item).css('transition-duration', '0s');
     // }

     // function stopSorting(e, ui) {


     //      self.reOrderPresetFilters(formObj);

     //      self.updatePresetFilters(formObj, true); //quiet update
     //      setTimeout(function () {
     //           $(ui.item).css('transition-duration', '');
     //      }, timeOutInterval)

     // }
}

this.destroySortablePresetFilters = function (formObj) {
     //$(formObj.presetWrapper).sortable("destroy")
}

