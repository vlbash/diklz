this.deletePresetFilter = function (e, formObj, el) {

     var msg = "<p>Підтвердіть видалення фільтру</p>",
          id = 'filterId',
          index = self.findObjIndexInArr(formObj.filters, id, $(el).attr('id'));

     self.createDialog(e, msg, deleteFilter, noRemoveFilter);

     function deleteFilter() {

          formObj.filters.splice(index, 1);
          self.clearElEventListeners(el);
          $(el).remove();
          self.reOrderPresetFilters(formObj);

          self.updatePresetFilters(formObj);

     }

     function noRemoveFilter() {
          return;
     }


};