(function($){
    jQuery.fn.ratingEditor = function(){
        var make = function () {

            var thisButton = this;
            //Перед элементом должен быть элемент hidden со значением рейтинга возвращаемым форме
            var hidden = $(thisButton).prev("input");
            //После элемента идет span с отображаемым значением рейтинга
            var span = $(thisButton).next("span");

            //Вставляем текстовое поле
            $(thisButton).after('<input type="text" class="form-control input-sm rating-input" value="' + $(span).html() + '" />');
            //Вставляем кнопку подтверждения изменения рейтинга
            $(thisButton).before('<a class="btn btn-default btn-xs rating-edit"><span class="glyphicon glyphicon-ok"></span></a>');

            var input = $(thisButton).next("input");
            var applyButton = $(thisButton).prev("a");

            $(input).after('<a class="btn btn-default btn-xs rating-edit"><span class="glyphicon glyphicon-remove"></span></a>');

            var cancelButton = $(input).next("a");

            $(input).hide();
            $(applyButton).hide();
            $(cancelButton).hide();

            $(thisButton).click(function () {
                $(thisButton).hide();
                $(span).hide();
                $(input).show();
                $(applyButton).show();
                $(cancelButton).show();
            });

            $(applyButton).click(function () {
                var newVal = $(input).val();
                if (newVal == "Нет оценки") {
                    newVal = -1;
                }
                else if (!(isFinite(newVal))) {
                    newVal = 0;
                }
                $(thisButton).show();
                $(span).show();
                $(span).html(newVal);
                $(hidden).val(newVal);
                $(input).hide();
                $(applyButton).hide();
                $(cancelButton).hide();
            });

            $(cancelButton).click(function () {
                $(thisButton).show();
                $(span).show();
                $(input).hide();
                $(input).val($(span).html());
                $(applyButton).hide();
                $(cancelButton).hide();
            });
            
        };
 
        return this.each(make); 
        
    };
})(jQuery);