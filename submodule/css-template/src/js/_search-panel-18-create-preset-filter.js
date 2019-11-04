this.openNewPresetFilterForm = function (formObj) {

     formObj.savePresetFilterFormOpen = true;

     self.addActiveClass($(formObj.savePresetFilterForm));
     $(formObj.savePresetFilterNameInput).focus();

     $(formObj.savePresetFilterCancelBtn).on('click', function (e) {
          e.preventDefault();
          self.closeNewPresetFilterForm(formObj)
     });

     $(formObj.savePresetFilterSaveBtn).on('click', function (e) {
          e.preventDefault();
          self.savePresetFilter(formObj);
     })

}

this.closeNewPresetFilterForm = function (formObj) {

     if (!formObj.savePresetFilterFormOpen) {
          return;
     }
     $(formObj.savePresetFilterNameInput).val('');
     self.removeActiveClass($(formObj.savePresetFilterForm));
     formObj.savePresetFilterFormOpen = false;
}