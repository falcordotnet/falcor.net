//logging:
var stringifyReplacer = function (key, value) {
    if (key === '\u001Eparent' || key === '\u001Ekey' || key === '\u001Epath') {
        return undefined;
    } else {
        return value;
    }
};
var jsonStringify = function (x) {
    return JSON.stringify(x, stringifyReplacer, 3);
}

var logJson = function (x) { console.log(jsonStringify(x)); };
var logJsonError = function (x) { console.error(jsonStringify(x)); };

