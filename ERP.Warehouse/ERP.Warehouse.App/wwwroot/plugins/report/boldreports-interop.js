
(function () {
    window.BoldReports = window.BoldReports || {};

    // Interop file to render the Bold Report Viewer component with properties.
    window.BoldReports.RenderViewer = function (elementID, reportViewerOptions) {
        var $viewer = $("#" + elementID);
        if ($viewer.length === 0) {
            return;
        }

        window.BoldReports.DestroyViewer(elementID);

        $viewer.boldReportViewer({
            reportPath: reportViewerOptions.reportName,
            reportServiceUrl: reportViewerOptions.serviceURL
        });
    };

    window.BoldReports.DestroyViewer = function (elementID) {
        var $viewer = $("#" + elementID);
        if ($viewer.length === 0) {
            return;
        }

        try {
            $viewer.boldReportViewer("destroy");
        } catch (e) {
            $viewer.empty();
        }
    };

    window.BoldReports.Download = function (reportId, formatType) {
        // var formatType = "";
        // if ($('#rbtnPDf').is(':checked')) {
        //     formatType = $('#rbtnPDf').val();
        // } else if ($('#rbtnWord').is(':checked')) {
        //     formatType = $('#rbtnWord').val();
        // } else if ($('#rbtnxls').is(':checked')) {
        //     formatType = $('#rbtnxls').val();
        // } else if ($('#rbtnCSV').is(':checked')) {
        //     formatType = $('#rbtnCSV').val();
        // }
        // formatType = formatType || "PDF";
        location.href = 'Export/Download?writerFormat=' + formatType + '&reportId=' + reportId;
    };
})();