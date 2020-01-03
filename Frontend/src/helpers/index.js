// An array is truthy only when it does not contain any falsy value
// like false, undefined, null, 0, '' and NaN
export function isArrayTruthy(array) {
    for (let i = 0; i < array.length; i++) {
        const item = array[i];
        if (!item) return false;
    }
    return true;
}
