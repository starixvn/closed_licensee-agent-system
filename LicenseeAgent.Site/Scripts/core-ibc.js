var core = core || {};
core.ibc = (function () {
    'use strict';

    return {
        cfsCode: function (nWord) {
            var result = "";
            for (var cc = 1; cc <= nWord.length; cc++) {
                result += nWord.charAt(cc - 1).charCodeAt(0);
            }
            var DecimalValue = Number(result);
            return DecimalValue.toString(16);
        },
        cfsEncode: function (codeStr) {
            var CodeLen = 30, CodeSpace, Been;
            CodeSpace = CodeLen - codeStr.length;
            if (CodeSpace > 1) {
                for (var cecr = 1; cecr <= CodeSpace; cecr++) {
                    codeStr = codeStr + String.fromCharCode(21);
                }
            }
            var NewCode = Number(1);

            for (var cecb = 1; cecb <= CodeLen; cecb++) {
                Been = CodeLen + codeStr.charAt(cecb - 1).charCodeAt(0) * cecb;
                NewCode = NewCode * Been;
            }

            var tmpNewCode = Number(NewCode.toPrecision(15)); //to convert to the same precision as c# code
            codeStr = tmpNewCode.toString().toUpperCase();
            var NewCode2 = "";

            var cec;
            for (cec = 1; cec <= codeStr.length; cec++) {
                NewCode2 = NewCode2 + core.ibc.cfsCode(codeStr.substring(cec - 1, cec + 2));
            }

            var CfsEncodeStr = "";
            for (cec = 20; cec <= NewCode2.length - 18; cec += 2) {
                CfsEncodeStr = CfsEncodeStr + NewCode2.charAt(cec - 1);
            }

            return CfsEncodeStr.toUpperCase();
        }
    };
} ());