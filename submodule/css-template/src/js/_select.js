///https://ru.stackoverflow.com/questions/564448/

this.closeAllSelect = function () {
   $('.select-gap').each(function () {
      self.closeDefinedSelect($(this));
   });
};

this.closeDefinedSelect = function (el) {

   if (!$(el).hasClass('select-gap')) {
      el = $(el).parent().find('.select-gap');
   }

   el.removeClass('on');

   var selectGroupInner = el.closest('.select-group-inner'),
      selectList = el.next('.select-list');

   if ($.trim(selectGroupInner.find('select').val()) != '' || $.trim(selectGroupInner.find('.select-gap').text()) != '') {
      selectGroupInner.addClass('not-empty');
   } else {
      selectGroupInner.removeClass('not-empty');
   }
   if ($(selectGroupInner).closest('.datepicker-group')) {
      self.addDatePickerGroupEmptyNotEmptyClass(selectGroupInner)

   }

   selectGroupInner.removeClass('on');
   selectList.slideUp(selectDuration);
   self.destroySelectScrollBar();
   selectList.removeAttr('id');
   selectOpen = false;
};

this.selectWrap = function (select) {

   if ($(select).is(':hidden') || !$(select).is(':visible')) {
      return
   }
   select.hide();
   // Wrap all
   select.wrap('<div class="select-group-inner"></div>');

   // Add decorative line
   $('<span>', {
      class: 'select-group-bar'
   }).insertAfter(select);

   var selectGroupBar = select.next('.select-group-bar');

   // Wrap all in select box
   $('<div>', {
      class: 'select-gap'
   }).insertAfter(selectGroupBar);
   var selectGap = selectGroupBar.next('.select-gap');
   // caret = selectGap.find('.caret');

   // Add ul list
   $('<ul>', {
      class: 'select-list'
   }).insertAfter(selectGap);

   self.createSelectList(select);
};

this.createSelectList = function (select) {
   var selectOption = select.find('option'),
      selectGroupInner = select.closest('.select-group-inner'),
      selectOptionLength = selectOption.length,
      selectWarning = false,
      selectGap = selectGroupInner.find('.select-gap'),
      selectList = selectGap.next('.select-list'),
      selectedOption = selectOption.filter(':selected');
   (selectGap).attr('tabindex', '0')
   if (selectedOption.length) {
      selectGap.text(selectOption.eq(selectedOption.index()).text());
      selectGroupInner.removeClass('select-readonly');
      if (!self.isStringEmpty($(selectGap).text())) {
         $(selectGroupInner).addClass('not-empty');
      }
   } else {
      selectGap.text('');
      selectGroupInner.addClass('select-readonly');
      selectGroupInner.removeClass('not-empty');
      return;
   }
   if ($(select).is('[readonly]')) {
      $(selectGroupInner).addClass('select-readonly')
      return;
   }
   if ($(selectGroupInner).closest('.datepicker-group')) {
      self.addDatePickerGroupEmptyNotEmptyClass(selectGroupInner)

   }

   selectDuration = select.css('transition-duration');

   if (select.hasClass('select-warning')) {
      selectWarning = true;
   }

   // Add li - option items
   for (var i = 0; i < selectOptionLength; i++) {
      $('<li>', {
            class: 'select-item',
            html: $('<span>', {
               text: selectOption.eq(i).text()
            })
         })
         .attr('data-value', selectOption.eq(i).val())
         .attr('data-disabled', selectOption.eq(i).attr('disabled'))
         .appendTo(selectList);
   }
   var selectItem = selectList.find('li');
   selectList.slideUp(0);

   selectGap.on('click keydown', function () {
      if (!$(this).hasClass('on')) {
         if (selectOpen) {
            self.closeAllSelect();
         }

         $(this).addClass('on');
         selectGroupInner.addClass('on');
         selectOpen = true;

         selectList.slideDown(selectDuration);

         self.createSelectScrollBar(selectList.get(0));

         selectItem.off().on('click keydown', function (e) {
            if (selectWarning && select.val()) {
               createSelectDialog(e, $(this));
            } else {
               setSelectValue($(this));
            }
         });
      } else {
         self.closeDefinedSelect($(this));
      }
   });

   function setSelectValue(el) {
      var chooseItem = el.data('value');
      $(select).val(chooseItem).attr('selected', 'selected');
      selectGap.text(el.find('span').text());
      self.closeDefinedSelect(selectGap);
      $(select).trigger('change');
   }

   function createSelectDialog(e, el) {
      var msg = '<p>При зміні сутності, введенні права доступу для полей сутності видаляться автоматично</p>';

      function confirmDialog() {
         return setSelectValue(el);
      }

      function refuseDialog() {
         return self.closeAllSelect();
      }
      self.createDialog(e, msg, confirmDialog, refuseDialog);
   }
};

this.removeAttrSelected = function (select) {
   $(select).val('');
   $(select).find('option').removeAttr('selected');
   select.closest('.select-group-inner').find('.select-gap').text('');
   self.closeDefinedSelect(select);
}

this.updateSelectList = function (select) {
   self.clearSelectGroupInner(select);
   self.createSelectList(select);
};

this.clearSelectGroupInner = function (select) {
   var selectGroupInner = select.closest('.select-group-inner');
   self.clearElEventListeners(selectGroupInner);
   $(selectGroupInner).find('.select-list').html('');
}


this.setSelectValue = function (select, value) {

   $(select).find('option').each(function () {
      $(this).removeAttr('selected');
   })
   $(select).find('[value = ' + value + ']').attr('selected', 'selected');
   self.updateSelectList(select);
}

this.returnSelectedOptionText = function (select) {
   var options = $(select).find('option'),
      currOption,
      selectValue = $(select).val();
   for (var i = 0; i < options.length; i++) {
      currOption = options[i];
      if ($(currOption).val() == selectValue) {
         return $(currOption).text();
      }
   }
}