// Add to wwwroot/js/pdfGenerator.js
window.generatePdfWithQr = function(dataJson) {
    const data = JSON.parse(dataJson);
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Title
    doc.setFontSize(24);
    doc.setTextColor(0, 0, 128); // Blue
    doc.text(data.Title, 105, 25, { align: 'center' });

    // Merchant Details
    doc.setFontSize(14);
    doc.setTextColor(0, 0, 0); // Black
    doc.text(`Merchant ID: ${data.MerchantId}`, 20, 50);
    doc.text(`Phone: ${data.Phone}`, 20, 60);
    doc.setTextColor(255, 140, 0); // Orange
    doc.text('Status: Test Merchant', 20, 70);

    // Add QR Code Image
    if (data.QrBase64) {
        try {
            // Convert base64 to proper format
            const qrData = data.QrBase64;
            doc.addImage(qrData, 'PNG', 80, 90, 50, 50);

            // Add QR label
            doc.setFontSize(12);
            doc.setTextColor(0, 0, 0);
            doc.text('Scan QR Code', 105, 145, { align: 'center' });
            doc.setFontSize(10);
            doc.text('For payment/verification', 105, 152, { align: 'center' });
        } catch (e) {
            console.error('Error adding QR to PDF:', e);
            doc.setFontSize(10);
            doc.text('QR Code not available', 80, 115);
        }
    }

    // Footer
    doc.setFontSize(8);
    doc.setTextColor(128, 128, 128);
    doc.text(`Generated: ${new Date().toLocaleString()}`, 10, 285);
    doc.text('Test Account - For Demonstration Only', 105, 285, { align: 'center' });

    // Save PDF
    doc.save(`Merchant_${data.MerchantId}.pdf`);
};