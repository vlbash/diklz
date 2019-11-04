// var pglist = $('[data-pgparams]');
// if (pglist) {
//     var pgparams = pglist.data('pgparams');
// }

// if (pgparams){
//     $(document).on('click', '.pagination>li>a, .content-list-wrapper[data-pgparams] .content-list-sortable', function(e) {
//         e.preventDefault();
//         $.ajax({
//             url: this.href + '&' + pgparams,
//             type: "GET",
//             success: function(data) {
//                 var tab = $("<div/>").html(data).find(".content-list-wrapper[data-pgparams]").html();
//                 $(".content-list-wrapper[data-pgparams]").html(tab);
//             }
//         });
//     });
// }

// $(document).off('submit', "form[data-ajax-update='#results']");
// $(document).on('submit', "form[data-ajax-update='#results']", function() {
//     $(document).off('click', '.pagination>li>a, .content-list-wrapper[data-pgparams] .content-list-sortable');
// });

