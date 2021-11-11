Colors = {};
Colors.names = {
    or1: "#efeeca99",
    b2: "#cde2e2c9",
    g1: "#12a679",
    p1: "#ff6384",
    b1: "#36a2eb",
    yg1: "#cddc39",
    p2: "#e91e63",
    y3: "#ff9f40",
    g2: "#009688",
    green: "#0ea77d",
    red: "#eb4c2d",
    blue: "#3b69aa",
    yellow: "#f3b801",
    orange: "#ffc800",
    violet: "#803d8c",
    purple: "#800080",
    pink: "#e0115e",
    aqua: "#00ffff",
    azure: "#f0ffff",
    beige: "#f5f5dc",
    brown: "#a52a2a",
    cyan: "#00ffff",
    darkblue: "#00008b",
    darkcyan: "#008b8b",
    darkgrey: "#a9a9a9",
    darkgreen: "#006400",
    darkkhaki: "#bdb76b",
    darkmagenta: "#8b008b",
    darkolivegreen: "#556b2f",
    darkorange: "#ff8c00",
    darkorchid: "#9932cc",
    darkred: "#8b0000",
    darksalmon: "#e9967a",
    darkviolet: "#9400d3",
    gold: "#ffd700",
    indigo: "#4b0082",
    khaki: "#f0e68c",
    lightblue: "#add8e6",
    lightcyan: "#e0ffff",
    lightpink: "#ffb6c1",
    lightyellow: "#ffffe0",
    lime: "#00ff00",
    magenta: "#ff00ff",
    maroon: "#800000",
    navy: "#000080",
    olive: "#808000",

};
Colors.select = function (index) {
    var result = null;
    var count = 0;
    for (var prop in this.names) {
        if (count == index) {
            result = prop;
            break;
        }
        count++;
    }
    if (!result) result = "red";

    return { name: result, rgb: this.names[result] };
};