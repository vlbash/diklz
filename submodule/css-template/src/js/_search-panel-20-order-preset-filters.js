this.reOrderPresetFilters = function (formObj) {

     var filterBtn = $(formObj.presetWrapper).find('.preset-btn'),
          currBtn,
          currObjIndex,
          id = 'filterId';

     for (var i = 0; i < filterBtn.length; i++) {
          currBtn = filterBtn[i];
          currObjIndex = self.findObjIndexInArr(formObj.filters, id, $(currBtn).attr('id'));
          formObj.filters[currObjIndex].order = i
     }
     if (!formObj.filters || !formObj.filters.length) {
          self.closePresetFiltersForm(formObj);
     }
}

this.findObjIndexInArr = function (arr, prop, value) {
     var valueTrimmed = $.trim(value);
     for (var i = 0; i < arr.length; i++) {
          var el = arr[i];
          if ($.trim(el[prop]) === valueTrimmed) {
               return i;
          }
     }
};

this.sortPresetFiltersByOrder = function (arr) {

     return self.sortArrObjByMethod(arr, 'order');
};