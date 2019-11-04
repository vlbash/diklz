$(document).ready(function () {
    $(document).on("change", "#LicenseType", function () {
        var licType = this;
        var msgType = $("#MessageType").val();
        var msgId = $("#Id").val();
        $.ajax({
            url: "GetMpdByLicense/?mpd=" + $(licType).val() + "&msgType=" + msgType + "&msgId=" + msgId,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                var mpd = "<option></option>";
                var slList = "<li class='select-item' data-value=''><span></span></li>";
                $(result).each(function () {
                    mpd += "<option value='" + this.value + "'>" + this.text + "</option>";
                    slList += '<li class="select-item" data-value="' + this.value + '"><span>' + this.text + '</span></li>';
                });
                var cmbMpd = $("#MPDGuidEnum");
                var selectList = $('#MPDGuidEnum').siblings("ul.select-list");
                var selectGap = $('#MPDGuidEnum').siblings(".select-gap");
                selectGap.text('');
                cmbMpd.empty();
                cmbMpd.append(mpd);
                selectList.empty();
                selectList.append(slList);
                $(cmbMpd).val();
                var selectItem = $('#MPDGuidEnum').siblings("ul.select-list").find("li");
                selectItem.off().on('click keydown', function (e) {
                    setSelectValue($(this));
                });

                function setSelectValue(el) {
                    var chooseItem = el.data('value');
                    $(cmbMpd).val(chooseItem).attr('selected', 'selected');
                    selectGap.text(el.find('span').text());
                    mt.closeDefinedSelect(selectGap);
                    $(cmbMpd).trigger('change');
                }

            },
            error: function (data) {
                return "Error";
            }
        });
    });
})