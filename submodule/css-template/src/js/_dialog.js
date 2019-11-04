this.createDialog = function(e, msg, yesCallback, noCallback) {
  var dialogInner = $("#dialog-inner");

    dialogOpen = true;

  if (yesCallback && noCallback){
    dialogInner
      .find(".btn-holder")
      .html(
        "<button id='dialog-refuse' class='btn btn-danger btn-outline'>Відмовитися</button><button id='dialog-confirm' class='btn btn-secondary btn-outline'>Підтвердити</button>"
      );

    setYesCallbackListener();
    setNoCallbackListener();
  } else if (noCallback) {
    dialogInner
      .find(".btn-holder")
      .html(
        "<button id='dialog-refuse' class='btn btn-danger btn-outline'>OK</button>"
      );
      
    setNoCallbackListener();
  } else if (yesCallback) {
    dialogInner
    .find(".btn-holder")
    .html("<button id='dialog-confirm' class='btn btn-secondary btn-outline'>Підтвердити</button>");

    setYesCallbackListener();
  } else {
    dialogInner
      .find(".btn-holder")
      .html(
        "<button id='dialog-confirm' class='btn btn-secondary btn-outline'>OK</button>"
      );

    setYesCallbackListener();
  }

  // if (e) {
  //   var X = e.pageX,
  //     Y = e.pageY,
  //     dialoginnerWidth = dialogInner.outerWidth(),
  //     dialoginnerHeight = dialogInner.outerHeight(),
  //     winWidth = self.calculateWindowWidth();
  //     winHeight = self.calculateWindowHeight();

  //   if (X + dialoginnerWidth > winWidth) {
  //     X = winWidth - dialoginnerWidth - 15;
  //   }
  //   if (Y + dialoginnerHeight > winHeight) {
  //     Y = winHeight - dialoginnerHeight - 15;
  //   }
  //   dialogInner.css({
  //     position: "absolute",
  //     top: Y - dialoginnerHeight + "px",
  //     left: X + "px"
  //   });
  // }

  // dialogInner.draggable();

  $("#dialog-text").html(msg);

  self.showDialogWrapper();

  function setYesCallbackListener() {
    $("#dialog-confirm")
      .off()
      .on("click", function() {
        self.hideDialogWrapper();
        if (yesCallback) yesCallback();
      });
  }

  function setNoCallbackListener() {
    $("#dialog-refuse")
      .off()
      .on("click", function() {
        self.hideDialogWrapper();
        noCallback();
      });
  }
};

this.showDialogWrapper = function() {
  self.addOpenClass(dialogWrapper);
  self.addActiveClass(dialogWrapper);
};

this.hideDialogWrapper = function() {
  self.removeActiveClass(dialogWrapper);
  setTimeout(function() {
    $("#dialog-confirm").off();
    $("#dialog-refuse").off();
    $("#dialog-text").html("");
    $("#dialog-btn").html("");
    // $("#dialog-inner").css({
    //   top: "",
    //   left: ""
    // });
    // $("#dialog-inner").draggable("destroy");
    self.removeOpenClass(dialogWrapper);
    dialogOpen = false;
  }, timeOutInterval);
};
