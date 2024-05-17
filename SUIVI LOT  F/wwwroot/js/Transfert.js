(function ($) {
    "use strict";
    //datepicker section
    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1; // Months are zero-indexed
    var year = currentDate.getFullYear();
    var formattedDate = (day < 10 ? '0' : '') + day + '/' + (month < 10 ? '0' : '') + month + '/' + year;

    $("#datepickerFrom").val(formattedDate);//initial dateFrom
    $("#datepickerTo").val(formattedDate);//initial dateTo

    var $min = $("#datepickerFrom").val(),
        $max = $("#datepickerTo").val(),
        $maxformat = $max.split("/"),
        $minformat = $min.split("/");

    const datepickerWithLimitsFROM = $('#datepicker-with-limits-From')[0];
    const datepickerWithLimitsTo = $('#datepicker-with-limits-To')[0];

    const sidenav = document.getElementById("datepicker-with-limits-From");
    sidenav.addEventListener("dateChange.te.datepicker", (event) => {
        //table.clear().draw();
        $minformat = $("#datepickerFrom").val().split("/");
        $("#datepickerTo").val($("#datepickerFrom").val());
        $To.update({ min: new Date(Number($minformat[2]), Number($minformat[1] - 1), Number($minformat[0])), startDay: 1, weekdaysNarrow: ["D", "L", "M", "M", "J", "V", "S"], });
    });

    const sidenav3 = document.getElementById("datepicker-with-limits-To");
    sidenav3.addEventListener("dateChange.te.datepicker", (event) => {
        //table.clear().draw();
    });

    var $From = new te.Datepicker(datepickerWithLimitsFROM, {
        format: "dd/mm/yyyy",
        title: "Sélectionner une date de Début",
        monthsFull: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre",
        ],
        monthsShort: ["Jan", "Fév", "Mar", "Avr", "Mai", "Juin", "Juil", "Aoû", "Sep", "Oct", "Nov", "Déc",
        ],
        weekdaysFull: ["Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi",
        ],
        weekdaysShort: ["Dim", "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam"],
        weekdaysNarrow: ["D", "L", "M", "M", "J", "V", "S"],
        okBtnText: "OK",
        startDay: 1,
        clearBtnText: "Effacer",
        cancelBtnText: "Fermer",
        removeClearBtn: true,
        confirmDateOnSelect: true,
        min: new Date(1997, 6, 7),
        max: new Date(Number($maxformat[2]), Number($maxformat[1] - 1), Number($maxformat[0])),
    });

    var $To = new te.Datepicker(datepickerWithLimitsTo, {
        title: "Sélectionner une date de Fin",
        format: "dd/mm/yyyy",
        monthsFull: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre",
        ],
        monthsShort: ["Jan", "Fév", "Mar", "Avr", "Mai", "Juin", "Juil", "Aoû", "Sep", "Oct", "Nov", "Déc",
        ],
        weekdaysFull: ["Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi",
        ],
        weekdaysShort: ["Dim", "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam"],
        weekdaysNarrow: ["D", "L", "M", "M", "J", "V", "S"],
        okBtnText: "OK",
        startDay: 1,
        clearBtnText: "Effacer",
        cancelBtnText: "Fermer",
        removeClearBtn: true,
        confirmDateOnSelect: true,
        min: $From.options.max,
        max: new Date()
    },
        {
            datepickerHeader: "xs:max-md:landscape:h-full h-[120px] px-6 bg-success-700 flex flex-col rounded-t-lg dark:bg-zinc-800"
        }
    );

    $(".historique").on("click", function () {
        $(".enseigne").text($(this).attr("id"));
        $("#fackname").val($(this).attr("data-fackname"));
        $('#tableHisto').children(':not(.noremove)').remove();
        resetRecap();
    });

    $("form").on("submit", function (event) {
        event.preventDefault();
        $(".generate").prop("disabled", true);
        $(".search-button").removeClass("opacity-0");
        let formData = new FormData();
        formData = new FormData($("form")[0]);

        $.ajax({
            url: "/Transfert/Historiques",
            type: "POST",
            xhrFields: {
                withCredentials: true,
            },
            data: formData,
            contentType: false,
            processData: false,
            cache: false,
        }).done(function (response) {
            // Handle success
            if (response.data && response.data.length < 1) {
                if (response.message && response.message.length < 1) {
                    Swal.fire({
                        title: 'Aucune donnée',
                        text: 'La liste des données est vide.',
                        icon: 'info',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Fermer',
                    });
                    $('#tableHisto').children(':not(.noremove)').remove();
                } else {
                    Swal.fire({
                        title: 'Exception',
                        text: response.message,
                        icon: 'warning',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Fermer',
                    });
                    $('#tableHisto').children(':not(.noremove)').remove();
                }
            } else {
                $('#tableHisto').children(':not(.noremove)').remove();
                var tableHiso = createTable(response.data);
                $('#tableHisto').append(tableHiso);
            }
            $(".search-button").addClass("opacity-0");
            $(".generate").prop("disabled", false);
        }).fail(function (error) {
            // Handle error
            $(".search-button").addClass("opacity-0");
            $(".generate").prop("disabled", false);
            $('#tableHisto').children(':not(.noremove)').remove();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'Fermer',
                text: "Une erreur est survenue",
                footer: "S'il vous plaît essayer à nouveau"
            });
            resetRecap();
        });
    });

    function createTable(data) {
        var datalength = data.length;
        let sumLotJour = 0;
        let sumPliJour = 0;
        let sumPlivalide = 0;
        let sumPlirejeter = 0;

        const htmlString = ((data ?? []).map((item, index) => {
            sumLotJour += item.lot_jour;
            sumPliJour += item.pli_jour;
            sumPlivalide += item.pli_valide;
            sumPlirejeter += item.pli_rejete;
            return `
            <tr class="dt-row hover:bg-gray-400 transition-colors duration-300 border-b ${index % 2 !== 0 ? `bg-gray-200` : ``}">
                <td class="whitespace-nowrap px-3 py-1 font-medium text-right">${formatDate(item.last_update)}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.lot_jour}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.pli_jour}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.pli_valide}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.pli_rejete}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.chq_pm1_valide}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.cba_valide}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.nb_alpha}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.nb_non_alpha}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.pli_manquant}</td>
                <td class="whitespace-nowrap px-3 py-1 text-right">${item.scanner}</td>
            </tr>`
        }).join(''))
        
        $("#lotjour").text(sumLotJour);
        $("#plijour").text(sumPliJour);
        $("#valide").text(sumPlivalide);
        $("#rejete").text(sumPlirejeter);
        return htmlString;
    }

    function formatDate(dateString) {
        var originalDate = new Date(dateString);
        var formattedDate = originalDate.toLocaleString('fr-FR', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        });
        return formattedDate;
    }

    function resetRecap() {
        $("#lotjour").text(0);
        $("#plijour").text(0);
        $("#valide").text(0);
        $("#rejete").text(0);
    }
})(jQuery)