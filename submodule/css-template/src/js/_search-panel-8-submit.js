this.submitSearchForm = function (formObj) {

     if (!self.validateForm($(formObj.formHolder).find('ul'))) {
          return;
     }

     self.showLoader($($(formObj.form).data('ajax-update')));

     self.clearMainInput(formObj);

     self.closeSearchFormHolder(formObj);
     // self.closeSearchFormPreset(formObj);
     self.clearPaging(formObj.form);

     formObj.shownFilters = self.findDataForPresetFilters(formObj);

     var queryData = self.prepareSearchQueryData(formObj.shownFilters);

     self.ajaxSubmitSearchForm(formObj, queryData);
};


this.submitDefaultFilter = function (formObj) {
     self.showLoader($($(formObj.form).data('ajax-update')));
     var filters = formObj.filters,
          defaultFilter = false;
     for (var i = 0; i < filters.length; i++) {
          if (filters[i].default) {
               defaultFilter = filters[i];
               break;
          }
     }
     if (defaultFilter) {
          formObj.shownFilters = defaultFilter.fields;
          var query = self.prepareSearchQueryData(defaultFilter.fields);
          self.ajaxSubmitSearchForm(formObj, query);
     } else {
          self.submitMainInput(formObj);
     }
};


this.findDataForPresetFilters = function (formObj) {

     var arr = [];

     var selectGroupChildren = $(formObj.formHolder).find('.select-group');
     var checkboxGroupChildren = $(formObj.formHolder).find('.checkbox-group');
     var inputGroupChildren = $(formObj.formHolder).find('.input-group');


     $(selectGroupChildren).each(function () {
          if ($(this).parent().is('.preset-filter-inner')) {
               return
          }
          setSelectGroupValue($(this))
     })

     $(checkboxGroupChildren).each(function () {
          if ($(this).parent().is('.preset-filter-inner')) {
               return
          }
          setCheckBoxGroupValue($(this))
     })

     $(inputGroupChildren).each(function () {
          if ($(this).parent().is('.preset-filter-inner')) {
               return
          }
          if ($(this).hasClass('datepicker-group')) {
               setDatePickerGroupValue($(this));
          } else {
               setInputGroupValue($(this))
          }

     })

     if (arr.length) {
          return arr;
     }
     return false;


     function setSelectGroupValue(el) {

          var select = $(el).find('select');

          if (!self.isElValueEmpty(select)) {
               arr.push({
                    'name': $(select).attr('name'),
                    'shownName': $(select).closest('.select-group').find('label').text(),
                    'type': 'select',
                    'value': $(select).val(),
                    'shownValue': self.returnSelectedOptionText(select)
               })
          }
     }



     function setCheckBoxGroupValue(el) {
          var checkbox = $(el).find('input:checkbox');
          if ($(checkbox).prop('checked')) {
               arr.push({
                    'name': $(checkbox).attr('name'),
                    'shownName': $(checkbox).parent().find('label').text(),
                    'type': 'checkbox',
                    'value': true,
                    'shownValue': 'Так'
               })
          }
     }

     function setInputGroupValue(el) {
          var input = $(el).find('input');

          if (!self.isElValueEmpty(input)) {
               arr.push({
                    'name': $(input).attr('name'),
                    'shownName': $(input).parent().find('label').text(),
                    'type': 'input',
                    'value': $(input).val(),
                    'shownValue': $(input).val()

               })
          }
     }

     function setDatePickerGroupValue(el) {
          var hiddenInput = $(el).find('.data-range-result');
          if (!self.isElValueEmpty(hiddenInput)) {
               if (!self.isElValueEmpty($(el).find('.select-range-input'))) {
                    arr.push({
                         'name': $(hiddenInput).attr('name'),
                         'shownName': $(hiddenInput).parent().find('label').text(),
                         'type': 'datePickerSelect',
                         'value': $(el).find('.select-range-input').val(),
                         'shownValue': self.returnSelectedOptionText($(el).find('.select-range-input')) + " тому"
                    })
               } else if (!self.isElValueEmpty($(el).find('.hasDatepicker'))) {
                    arr.push({
                         'name': $(hiddenInput).attr('name'),
                         'shownName': $(hiddenInput).parent().find('label').text(),
                         'type': 'datePickerInput',
                         'value': $(hiddenInput).val(),
                         'shownValue': $(el).find('.hasDatepicker').val()
                    })
               }
          }
     }

}


this.submitPresetFilter = function (formObj, filterBtn) {
     var id = 'filterId';
     var index = self.findObjIndexInArr(formObj.filters, id, $(filterBtn).attr('id')),
          queryData = self.prepareSearchQueryData(formObj.filters[index].fields);
     self.showLoader($($(formObj.form).data('ajax-update')));

     formObj.shownFilters = formObj.filters[index].fields;

     self.closePresetFiltersForm(formObj);

     self.ajaxSubmitSearchForm(formObj, queryData);
};


this.prepareSearchQueryData = function (fieldsArr) {

     var query = {
          "X-Requested-With": "XMLHttpRequest"
     };

     for (var i = 0; i < fieldsArr.length; i++) {
          if (fieldsArr[i].type == 'datePickerSelect') {
               query[fieldsArr[i].name] = self.subtractDate(new Date().toISOString(), fieldsArr[i].value);
          } else {
               query[fieldsArr[i].name] = fieldsArr[i].value
          }
     }
     return query;
}

this.ajaxSubmitSearchForm = function (formObj, queryData) {
     self.showLoader();
     var requestType = formObj.formMethod,
          url = $(formObj.form).attr('action'),
          formResult = $(formObj.form).data('ajax-update');

     self.clearPaging(formObj.form);
     $.ajax({
          type: requestType,
          url: url,
          data: {
               paramList: queryData,
               options: null
          },
          success: function (data) {
               $(formResult).html(data);
               self.searchFormCompleteFunction(formObj.form);
               self.prepareParametersFilter(formObj);
          },
          error: function (data) {
               var msg = '<p>Виникла помилка при виведенні данних пошуку</p>';
               self.createDialog(false, msg);
          }
     });

}

this.searchFormCompleteFunction = function (form) {

     var result = $($(form).data('ajax-update'));
     $.when(self.initMask(result))
          .done(self.findFormatDateParagraph(result))
          .done(self.findGrids(result))
          .done(self.createPaging(result))
          .done(
               self.setDeleteContentListItem(result),
               self.updateNiceScroll(),
               self.createCheckboxLabel(result),
               self.setToolTipsForChild(result),
               self.hideLoader(result)
          );
};

this.addSearchFormEnterListener = function (formObj) {

     $(document).keydown(function (e) {
          if (e.keyCode === 13 && searchFormOpen) {
               e.preventDefault();
               self.submitSearchForm(formObj);
          }
     });
};