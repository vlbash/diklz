
var arrUkr = new Array('А', 'а', 'Б', 'б', 'В', 'в', 'Г', 'г', 'Ґ', 'ґ',
                       'Д', 'д', 'Е', 'е', 'Є', 'є', 'Ж', 'ж', 'З', 'з',
                       'И', 'и', 'І', 'і', 'Ї', 'ї', 'Й', 'й', 'К', 'к',
                       'Л', 'л', 'М', 'м', 'Н', 'н', 'О', 'о', 'П', 'п',
                       'Р', 'р', 'С', 'с', 'Т', 'т', 'У', 'у', 'Ф', 'ф',
                       'Х', 'х', 'Ц', 'ц', 'Ч', 'ч', 'Ш', 'ш', 'Щ', 'щ',
                       'Ю', 'ю', 'Я', 'я', 'Ь', 'ь', '\'', '-', 'Ы', 'ы', 'Ё', 'ё');

var arrEn = new Array('A', 'a', 'B', 'b', 'V', 'v', 'H', 'h', 'G', 'g',
                      'D', 'd', 'E', 'e', 'Ye', 'ye', 'Zh', 'zh', 'Z', 'z',
                      'Y', 'y', 'I', 'i', 'Yi', 'yi', 'Y', 'y', 'K', 'k',
                      'L', 'l', 'M', 'm', 'N', 'n', 'O', 'o', 'P', 'p',
                      'R', 'r', 'S', 's', 'T', 't', 'U', 'u', 'F', 'f',
                      'Kh', 'kh', 'Ts', 'ts', 'Ch', 'ch', 'Sh', 'sh', 'Shch', 'shch',
                      'Yu', 'yu', 'Ya', 'ya', '', '', '', '-', 'Y', 'y', 'Ye', 'ye');

function cyrillicToLatin(text) {

    var letterCombination1 = text.indexOf('Зг');
    var letterCombination2 = text.indexOf('зг');

    if (letterCombination1 > -1) {
        text = text.replace('Зг', 'Zgh');
    }
    if (letterCombination2 > -1) {
        text = text.replace('зг', 'zgh');
    }

    for (var i = 0; i < arrUkr.length; i++) {
        var reg = new RegExp(arrUkr[i], "g");

        if (arrUkr[i] === 'ї' && text[0] !== 'Ї') {
            text = text.replace(reg, arrEn[23]);
        }
        if (arrUkr[i] === 'є' && text[0] !== 'Є') {
            text = text.replace(reg, 'ie');
        }
        if (arrUkr[i] === 'ю' && text[0] !== 'Ю') {
            text = text.replace(reg, 'iu');
        }
        if (arrUkr[i] === 'я' && text[0] !== 'Я') {
            text = text.replace(reg, 'ia');
        }
        
        text = text.replace(reg, arrEn[i]);
    }
    
    return text;
    
}


