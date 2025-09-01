onScan.attachTo(document, {
    suffixKeyCodes: [13],
    reactToPaste: false,
    onScan: function (sCode, iQty) {
        var barcode = document.getElementById('barcode');
        barcode.value = sCode;
        var changeEvent = new Event('change');
        barcode.dispatchEvent(changeEvent);
        console.log('barcode: ' + sCode);
    }
});