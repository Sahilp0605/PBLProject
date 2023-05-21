jQuery(document).ready(function () {
    /*
    .   Match any character except newline
    \w  Match any alphanumeric character
    \s  Match any whitespace character
    \d  Match any digit
    \b  Match the beginning or end of a word
    ^   Match the beginning of the string
    $   Match the end of the string
    --------------------------------------------------------------------------
    *       Repeat any number of times
    +       Repeat one or more times
    ?       Repeat zero or one time
    {n}     Repeat n times
    {n,m}   Repeat at least n, but no more than m times
    {n,}    Repeat at least n times
    --------------------------------------------------------------------------
    \W          Match any character that is NOT alphanumeric
    \S          Match any character that is NOT whitespace
    \D          Match any character that is NOT a digit
    \B          Match a position that is NOT the beginning or end of a word
    [^x]        Match any character that is NOT x
    [^aeiou]    Match any character that is NOT one of the characters aeiou
    --------------------------------------------------------------------------
    (exp)           Match exp and capture it in an automatically numbered group
    (?<name>exp)    Match exp and capture it in a group named name
    (?:exp)         Match exp, but do not capture it
    --------------------------------------------------------------------------
    >>> Lookarounds
    ---------------------------------
    (?=exp)         Match any position preceding a suffix exp
    (?<=exp)        Match any position following a prefix exp
    (?!exp)         Match any position after which the suffix exp is not found
    (?<!exp)        Match any position before which the prefix exp is not found
    --------------------------------------------------------------------------
    >>> Comment
    ---------------------------------
    (?#comment)     Comment
    --------------------------------------------------------------------------
    */

    function isValidDate(sText) {
        //var reDate = /(?:0[1-9]|[12][0-9]|3[01])\/(?:0[1-9]|1[0-2])\/(?:19|20\d{2})/;
        var reDate = /(?:0[1-9]|[12][0-9]|3[01])\/(?:0[1-9]|1[0-2])\/(?:20\d{2})/;
        return reDate.test(sText);
    }

    jQuery(function () {
        jQuery('.OnlyNumeric').keydown(function (e) {
            if (e.shiftKey || e.ctrlKey || e.altKey) {
                e.preventDefault();
            }
            else {
                var key = e.keyCode;
                if (!((key == 8) || (key == 9) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                    e.preventDefault();
                }
            }
        });

        jQuery('.OnlyDecimal').keydown(function (e) {
            if (e.shiftKey || e.ctrlKey || e.altKey) {
                e.preventDefault();
            }
            else {
                var key = e.keyCode;
                if (!((key == 8) || (key == 9) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                    e.preventDefault();
                }
            }
        });

        jQuery('.OnlyAlpha').keydown(function (e) {
            if (e.shiftKey || e.ctrlKey || e.altKey) {
                e.preventDefault();
            }
            else {
                var key = e.keyCode;
                if (!((key == 8) || (key == 9) || (key == 32) || (key == 127) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122))) {
                    e.preventDefault();
                }
            }
        });
    });

    /* For Ascii Table refer .... http://www.asciitable.com/ */

});
