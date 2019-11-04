this.initMask = function (el) {
  var tagName;

  $(el).find('.mask').each(function () {
    tagName = self.returnTagName($(this));

    if (tagName == 'input') {
      self.setInputMask($(this));
    }
    if (tagName == 'p') {
      self.setMaskedParagraph($(this));
    }
  });
};

this.setInputMask = function (el) {
  var inputParent = $(el).parent(),
    elName = $(el).attr('name'),
    elDd = $(el).attr('id'),
    clonedInput,
    position;

  $(inputParent).append('<input id = "' + elDd + '" class = "data-mask" type = "hidden" name = "' + elName + '"/>');

  $(el).attr('name', '');
  $(el).attr('id', '');

  clonedInput = $(inputParent).find('[name = ' + elName + ']');

  if (!self.isElValueEmpty(el)) {
    setInputValue();
  }
  setMaskedInputListeners();

  function setMaskedInputListeners() {
    var _value;

    $(el).focus(function () {
      _value = $(el).val();
      _value = _value.replace(/\s/g, '');
      _value = _value.replace(/,/g, '.');
      $(el).val(_value);
    });

    $(el).blur(setInputValue);

    $(el).on('input', function () {
      position = self.getCaretPosition($(el)[0]);
      _value = $(el).val();

      _value = _value.replace(/,/g, '.');

      var regexp = /^[0-9]{0,12}([\.][0-9]{0,2})?$/;

      if (!regexp.test(_value)) {
        _value = _value.substring(0, position - 1) + _value.substring(position, _value.length);
        window.setTimeout(function () {
          self.setCaretPosition($(el)[0], position - 1);
          position = self.getCaretPosition($(el)[0]);
        }, 0);
      }
      $(el).val(_value);
    });
  }

  function setInputValue() {
    var _value = $(el).val(),
      _hiddenValue = _value.replace(/\s/g, '');
    _hiddenValue = _hiddenValue.replace(/,/g, '.');

    var _arr = _hiddenValue.split('.');
    var _int = _arr[0];
    var _dec = _arr[1];

    if ($(el).hasClass('mask-money-dec')) {
      _dec = self.returnDec(_dec, 2);
    }
    _int = _int.replace(/(\d)(?=(\d\d\d)+([^\d]|$))/g, '$1 ');

    _value = _int + '.' + _dec;

    $(clonedInput).val(_hiddenValue);

    $(el).val(_value);
  }

  // function getCaretPosition(ctrl) {
  //      if (ctrl.selectionStart || ctrl.selectionStart == '0') {
  //           return ctrl.selectionStart;
  //      } else {
  //           return 0;
  //      }
  // }

  // function setCaretPosition(elem, caretPos) {
  //      if (elem != null) {
  //           if (elem.createTextRange) {
  //                var range = elem.createTextRange();
  //                range.move('character', caretPos);
  //                range.select();
  //           } else {
  //                if (elem.selectionStart) {
  //                     elem.focus();
  //                     elem.setSelectionRange(caretPos, caretPos);
  //                } else elem.focus();
  //           }
  //      }
  // }
};


this.getCaretPosition = function (ctrl) {
  if (ctrl.selectionStart || ctrl.selectionStart == '0') {
    return ctrl.selectionStart;
  } else {
    return 0;
  }
}

this.setCaretPosition = function (elem, caretPos) {
  if (elem != null) {
    if (elem.createTextRange) {
      var range = elem.createTextRange();
      range.move('character', caretPos);
      range.select();
    } else {
      if (elem.selectionStart) {
        elem.focus();
        elem.setSelectionRange(caretPos, caretPos);
      } else elem.focus();
    }
  }
}

this.setMaskedParagraph = function (el) {
  var value = $(el).text(),
    arr,
    int,
    dec;
  value = value.replace(/,/g, '.');
  arr = value.split('.');
  int = arr[0];
  dec = arr[1];

  if ($(el).hasClass('mask-money-dec')) {
    dec = self.returnDec(dec, 2);
  }

  int = int.replace(/(\d)(?=(\d\d\d)+([^\d]|$))/g, '$1 ');

  $(el).text(int + '.' + dec);
};

this.returnDec = function (str, decLength) {
  if (decLength == 2) {
    var temp;
    if (!str || typeof str == 'undefined' || str.length == 0 || isNaN(+str)) {
      return '00';
    }
    if (str.length == 1) {
      return str + '0';
    }
    if (str.length > 2) {
      str = str.split('');
      temp = +str[2];
      if (temp > 4) {
        str[1] = +str[1] + 1;
      }
      return str[0] + str[1];
    }
    return str;
  }
};