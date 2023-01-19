export const numericPattern = /^\d+$/;
export const decimalPattern = /^(\d+\.?\d{0,9}|\.\d{1,9})$/;
export const persianPattern = /^[\u0600-\u06FF\s]+$/;
export const allowedCharKey = [8, 46, 37, 38, 39, 40];
export const allowedCommandKey = [65, 97, 67, 99, 86, 117, 88, 120];

export const onKeyPress = (event) => {
    const keyCode = event.which || event.keyCode;
    const charCode = event.charCode;
    // allow ctrl+a,x,c,v
    if (event.ctrlKey) {
        if (allowedCommandKey.indexOf(keyCode) >= 0) {
            return true;
        }
    }
    // allow arrow key and delete and backspace
    if (allowedCharKey.indexOf(keyCode) >= 0) {
        return true;
    }
    const value = String.fromCharCode(charCode);
    if (!/[0-9.-]/.test(value)) {
        event.preventDefault();
        return false;
    } else {
        return true;
    }
};

export const onKeyPressTel = (event) => {
    const keyCode = event.which || event.keyCode;
    const charCode = event.charCode;
    // allow ctrl+a,x,c,v
    if (event.ctrlKey) {
        if (allowedCommandKey.indexOf(keyCode) >= 0) {
            return true;
        }
    }
    // allow arrow key and delete and backspace
    if (allowedCharKey.indexOf(keyCode) >= 0) {
        return true;
    }
    const value = String.fromCharCode(charCode);
    if (!/[0-9+]/.test(value)) {
        event.preventDefault();
        return false;
    } else {
        return true;
    }
};
