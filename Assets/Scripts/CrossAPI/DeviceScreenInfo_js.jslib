mergeInto(LibraryManager.library, {
    MeasureWebDPI: function () {
        let dpr = window.devicePixelRatio || 1;
        // create an empty element
        let div = document.createElement("div");
        // give it an absolute size of one inch
        div.style.width = "1in";
        // append it to the body
        let body = document.getElementsByTagName("body")[0];
        body.appendChild(div);
        // read the computed width
        let ppi = document.defaultView.getComputedStyle(div, null).getPropertyValue('width');
        // remove it again
        body.removeChild(div);
        // and return the value
        return parseFloat(ppi) * dpr;
    }
})

