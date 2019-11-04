this.setDefaultPresetFilter = function (formObj, el) {
     var inputId = '',
          inputChecked = false,
          id = 'filterId',
          index = self.findObjIndexInArr(formObj.filters, id, $(el).attr('id'));

     if ($(el).find('input:checkbox').prop('checked')) {
          inputId = $(el).find('input:checkbox').attr('id');
          inputChecked = true;
     }

     for (var i = 0; i < formObj.filters.length; i++) {
          formObj.filters[i].default = false;
     }

     $(formObj.presetWrapper).find('.checkbox').each(function () {
          $(this).prop('checked', false);
     });

     if (inputChecked) {
          $('#' + inputId).prop('checked', true);
          formObj.filters[index].default = true;
     }

     self.updatePresetFilters(formObj, true);
};