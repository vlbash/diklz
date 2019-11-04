FormValidation = function (form) {
  var validationContainer = $(form).find('.dh-validate'),
    validToken,
    cityzenBirtday,
    alarmMsg = [
      'Це поле має бути заповнене', //0
      'Значення має бути більше нуля', //1
      'Довжина - 2 символи', //2
      'Мають бути кирилличні символи', //3
      'У номері повинні бути лише цифри', //4
      'Довжина - 6 символів', //5
      'Довжина - 9 символів', //6
      'Не може бути менше поточної дати', //7
      'Довжина - 10 символів', //8
      'Довжина - 5 символів', //9
      'Адреса електроної пошти', //10
    ],
    currAlarm;


  if (!validationContainer.length) {
    return;
  } else {
    $(validationContainer).each(function () {
      $(this).append('<span class = "dh-alarm-msg"></span>');
      destroyHideMsgListener($(this));
    });
  }

  this.buttonClickFunc = function () {

    validToken = true;

    validationContainer.each(function () {
      $(this).off();
      checkElValue($(this));
    });

    return validToken;
  };

  function checkElValue(el) {
    if ($(el).hasClass('dh-validate-not-empty')) {
      checkNotEmptyEl(el);
    }
    if ($(el).hasClass('dh-validate-over-zero')) {
      checkOverZeroEl(el);
    }
    if ($(el).hasClass('dh-validate-passport-prefix')) {
      checkPasswordPrefix(el);
    }
    if ($(el).hasClass('dh-validate-passport-number')) {
      checkPasswordNumber(el);
    }
    if ($(el).hasClass('dh-validate-post-index')) {
      checkPostIndex(el);
    }
    if ($(el).hasClass('dh-validate-email')) {
      checkEmail(el);
    }
    if ($(el).hasClass('dh-validate-ipn')) {
      if (checkSixteen) {
        checkIpn(el);
      }
    }
  }

  function checkNotEmptyEl(el) {
    var elField;
    if ($(el).find('select').length) {
      elField = $(el).find('select');
    } else if ($(el).find('input').length) {
      elField = $(el).find('input');

    }

    if ($.trim(elField.val()) == '') {
      currAlarm = alarmMsg[0];
      setAlarmMsg(el);
      showAlarmMsg(el);
      validToken = false;
    }
  }

  function checkOverZeroEl(el) {
    if (+$(el).find('input').val() <= 0) {
      currAlarm = alarmMsg[1];
      setAlarmMsg(el);
      showAlarmMsg(el);
      validToken = false;
    }
  }

  function checkPasswordNumber(el) {
    var elFieldVal = $.trim($(el).find('input').val()),
      regExp = /^\d+$/,
      localToken = false;

    if (!regExp.test(elFieldVal)) {
      currAlarm = alarmMsg[4];
      localToken = true;
    }

    //if (elFieldVal.length != 6) {
    //    //currAlarm = alarmMsg[5];
    //    localToken = true;
    //}

    if (elFieldVal == '') {
      currAlarm = alarmMsg[0];
      localToken = true;
    }

    if (localToken) {
      setAlarmMsg(el);
      showAlarmMsg(el);
      validToken = false;
    }
  }

  function checkPostIndex(el) {
    var elFieldVal = $.trim($(el).find('input').val()),
      regExp = /^\d+$/,
      localToken = false;

    if (!regExp.test(elFieldVal)) {
      currAlarm = alarmMsg[4];
      localToken = true;
    }

    if (elFieldVal.length != 5) {
      currAlarm = alarmMsg[9];
      localToken = true;
    }

    if (elFieldVal == '') {
      currAlarm = alarmMsg[0];
      localToken = true;
    }

    if (localToken) {
      setAlarmMsg(el);
      showAlarmMsg(el);
      validToken = false;
    }
  }

  function checkPasswordPrefix(el) {
    var elFieldVal = $.trim($(el).find('input').val()),
      regExp = /([а-яґєії`´ʼ’ʼ’]+)/ui,
      localToken = false;

    if (!regExp.test(elFieldVal)) {
      currAlarm = alarmMsg[3];
      localToken = true;
    }
    //if (elFieldVal.length != 2) {
    //    currAlarm = alarmMsg[2];
    //    localToken = true;
    //}

    //if (elFieldVal == '') {
    //    currAlarm = alarmMsg[0];
    //    localToken = true;
    //}

    if (localToken) {
      setAlarmMsg(el);
      showAlarmMsg(el);
      validToken = false;
    }
  }

  function checkEmail(el) {
    var elFieldVal = $.trim($(el).find('input').val()),
      regExp = /^[-._a-z0-9]+@(?:[a-z0-9][-a-z0-9]+\.)+[a-z]{2,6}$/,
      localToken = false;

    if (!regExp.test(elFieldVal)) {
      currAlarm = alarmMsg[10];
      localToken = true;
    }
    if (elFieldVal == '') {
      currAlarm = alarmMsg[0];
      localToken = true;

    }

    if (localToken) {
      setAlarmMsg(el);
      showAlarmMsg(el);
      validToken = false;
    }

  }

  function checkSixteen() {
    var birthday = $('#CitizenBirthday');
    var arr = $(birthday).val().split('.');
    var checkDate = arr[2] + '.' + arr[1] + '.' + arr[0];
    var result = (new Date() - new Date(checkDate)); // 31557600; sec in year
    if (result > 16) {
      return true;
    }
    return false;
  }

  function checkIpn(el) {
    if ($('.dh-validate-no-ipn').prop('checked'))
      return;

    var elFieldVal = $.trim($(el).find('input').val()),
      regExp = /^\d+$/,
      localToken = false;

    if (!regExp.test(elFieldVal)) {
      currAlarm = alarmMsg[4];
      localToken = true;

    }
    if (elFieldVal.length != 10) {
      currAlarm = alarmMsg[8];
      localToken = true;
    }

    if (elFieldVal == '') {
      currAlarm = alarmMsg[0];
      localToken = true;
    }

    if (localToken) {
      setAlarmMsg(el);
      showAlarmMsg(el);
      validToken = false;
    }
  }

  function setAlarmMsg(el) {
    $(el).find('.dh-alarm-msg').text(currAlarm);
  }

  function createHideMsgListener(el) {
    $(el).on('mouseenter click focus', function () {
      hideAlarmMsg(el);
    });
    if ($(el).find('select').length) {
      $(el).find('select').on('focus', function () {
        hideAlarmMsg(el);
      })
    } else if ($(el).find('input').length) {
      $(el).find('input').on('focus', function () {
        hideAlarmMsg(el);
      })
    }
  }

  function destroyHideMsgListener(el) {
    $(el).off('mouseenter click focus', function () {
      hideAlarmMsg(el);

    });

    if ($(el).find('select').length) {

      $(el).find('select').off('focus', function () {
        hideAlarmMsg(el);
      })
    } else if ($(el).find('input').length) {
      $(el).find('input').off('focus', function () {
        hideAlarmMsg(el);
      })

    }
  }

  function showAlarmMsg(el) {
    $(el).addClass('dh-invalid');
    createHideMsgListener(el);
  }

  function hideAlarmMsg(el) {
    $(el).removeClass('dh-invalid');
    destroyHideMsgListener(el);
  }
};