this.uploadFile = function (el) {
    var form = $(el).closest('form');
    var url = $(form).attr('action');
    var entityId = $(form).find('#EntityId').val();
    var entityName = $(form).find('#EntityName').val();
    var docType = $(form).find('#DocumentType').val();
    var description = $(form).find('#Description').val();
    var files = $(form).find('#files').get(0).files;
    var data = new FormData();

    //return if there aren't files
    if (!files.length) {
        msg = "<p>Відсутні вкладені файли</p>";
        self.createDialog(true, msg);
        return;
    }
    
    var errMsg = function(fileSize, fileName){
        var num = fileSize / 1024 / 1024,
            currFileSize = num.toFixed(2),
            currFileName = fileName,
            msg = "<p>Розмір вкладеного файлу " + currFileName + ' ' + currFileSize + ' MB' + "</p><p>Максимальний розмір файлу має бути не більше 100 MB.</p>";
        self.createDialog(true, msg); 
        return;           
    }

    //include files
    for (var i = 0; i < files.length; i++) {
        data.append(files[i].name, files[i]);
        data.append("EntityId", entityId);
        data.append("EntityName", entityName);
        data.append("DocumentType", docType);
        data.append("Description", description);

        //check files size       
        if (files[i].size > 1e+8) {           
            errMsg(files[i].size, files[i].name);          
        }
    }

    var parent = $(form).closest(".upload-edit");
    var fileList = $(parent).find('.fileList');
    self.showLoader(fileList);

    //submit files
    $.ajax({
        type: "POST",
        url: url,
        contentType: false,
        processData: false,
        data: data,
        success: function (data) {      
            if (data.success == false){                
                self.hideLoader(fileListContainer);   
                errMsg(data.fileSize, data.fileName);                                          
              
            }else{
                $.ajax({
                    type: "Get",
                    url: '/FileStore/List?EntityId=' + entityId + '&amp;EntityName=' + entityName,
                    success: function (d2) {                  
                        var fileListContainer = fileList;
                        if (fileListContainer) {
                            $(fileListContainer).html(d2);
                            self.manageContentReload($(fileListContainer));
                            self.hideLoader(fileListContainer);
                            return true;
                        }
                        
                    }
                });
            }            
        }
    });

    //clean form
    $(form).trigger("reset");
    $('.content-upload-filename').html('');
    $('#Description').removeClass('not-empty').addClass('empty');
}

$(document).on('submit', '.form-file-upload', function (e) {
    e.preventDefault();
    self.uploadFile($(this));
});