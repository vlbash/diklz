this.prepareParametersFilter = function (formObj) {
     if (formObj.shownFilters.length) {
          $(formObj.clearAllFiltersBtn).on('click', function () {
               self.clearAllSearchFormFilters(formObj);
          });
          $.when($(formObj.parametersFilterHolder).find('ul').html(self.createPresetFilters(formObj.shownFilters))).done(
               $(formObj.parametersFilterHolder).find('.content-filter-parameters-item span').each(function () {
                    self.setRemoveFilterListener($(this), formObj.shownFilters, formObj);
               })
          )
          self.addOpenClass(formObj.parametersFilterHolder);
          self.addActiveClass(formObj.parametersFilterHolder);
          
     } else {
          $(formObj.parametersFilterHolder).find('ul').html('');
          self.removeOpenClass(formObj.parametersFilterHolder);
          self.removeActiveClass(formObj.parametersFilterHolder);
     }
}
this.createPresetFilters = function(arr) {
     var htmlInner = '',
          currFilter;
     for (var i = 0; i < arr.length; i++) {
          currFilter = arr[i];
          htmlInner += '<li class = "content-filter-parameters-item"> <p>' +
               currFilter.shownName + ': ' + currFilter.shownValue +
               '</p> <span data-remove = "' + currFilter.name + '"><i class="icon-xxs icon-close"></i></span>'
     }
     return htmlInner;
}

this.setRemoveFilterListener = function(el, arr, formObj) {
     var index,
          name = 'name';
     $(el).on('click', function () {
          index = self.findObjIndexInArr(arr, name, $(el).data('remove'));
          arr.splice(index, 1);
          var queryData = self.prepareSearchQueryData(arr);
          self.ajaxSubmitSearchForm(formObj, queryData);
     });
}

this.clearAllSearchFormFilters = function (formObj) {
     formObj.shownFilters = [];
     var queryData = self.prepareSearchQueryData(formObj.shownFilters);
     self.ajaxSubmitSearchForm(formObj, queryData);
}