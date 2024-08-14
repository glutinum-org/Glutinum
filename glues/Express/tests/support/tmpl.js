import { readFile } from 'fs';

var variableRegExp = /\$([0-9a-zA-Z\.]+)/g;

export default function renderFile(fileName, options, callback) {
    function onReadFile(err, str) {
        if (err) {
            callback(err);
            return;
        }

        try {
            str = str.replace(variableRegExp, generateVariableLookup(options));
        } catch (e) {
            err = e;
            err.name = 'RenderError'
        }

        callback(err, str);
    }

    readFile(fileName, 'utf8', onReadFile);
};

function generateVariableLookup(data) {
    return function variableLookup(str, path) {
        var parts = path.split('.');
        var value = data;

        for (var i = 0; i < parts.length; i++) {
            value = value[parts[i]];
        }

        return value;
    };
}
