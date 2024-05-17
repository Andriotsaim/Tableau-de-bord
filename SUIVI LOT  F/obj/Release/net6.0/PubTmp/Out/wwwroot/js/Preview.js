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
        $("#lotstraites").text(0);
        $("#plistraites").text(0);
        $('#accordion').children(':not(.noremove)').remove();
        $minformat = $("#datepickerFrom").val().split("/");
        $("#datepickerTo").val($("#datepickerFrom").val());
        $To.update({ min: new Date(Number($minformat[2]), Number($minformat[1] - 1), Number($minformat[0])), startDay: 1, weekdaysNarrow: ["D", "L", "M", "M", "J", "V", "S"], });
    });

    const sidenav3 = document.getElementById("datepicker-with-limits-To");
    sidenav3.addEventListener("dateChange.te.datepicker", (event) => {
        //table.clear().draw();
        $("#lotstraites").text(0);
        $("#plistraites").text(0);
        $('#accordion').children(':not(.noremove)').remove();
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

    
    ////// form with two dates
    $("form").on("submit", function (event) {
        event.preventDefault();
        $(".generate").prop("disabled", true);
        $(".search-button").removeClass("opacity-0");

        // Créez un objet FormData vide
        let formData = new FormData();
        formData = new FormData($("form")[0]);
        let checkedInputs = $('#enseignefilter input:checked');
        checkedInputs.each(function () {
            formData.append($(this).attr('name'), $(this).val());
        });
        $('#accordion').children(':not(.noremove)').remove();
        $.ajax({
            url: "/Preview/Previews",
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
                Swal.fire({
                    title: 'Aucune donnée',
                    text: 'La liste des données est vide.',
                    icon: 'warning',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'Fermer',
                });
            } else {
                //console.log(response.data);
                var lotstraites = 0;
                var plistraites = 0;
                $('#accordion').children(':not(.noremove)').remove();
                $.each(response.data, function (index, data) {
                    var accordionItem = createAccordionItem(index,data);
                    $('#accordion').append(accordionItem);
                    lotstraites = lotstraites + data.nbLotTraite;
                    plistraites = plistraites + data.nbplisglobal - data.nbPlinonTraites - data.nbPliRejected;
                });
                $("#lotstraites").text(lotstraites);
                $("#plistraites").text(plistraites);
                var tousLesTD = $("td");
                // Parcourir chaque élément <td> trouvé
                tousLesTD.each(function () {
                    //if ($(this).text().indexOf('IMP') !== -1) {
                    //    $(this).addClass('color-IMP');
                    //}
                    if ($(this).text().indexOf('EN COURS') !== -1) {
                        $(this).addClass('color-COURS');
                    }
                    if ($(this).text().indexOf('SAISI') !== -1) {
                        $(this).addClass('color-SAISI');
                    }
                    if ($(this).text().indexOf('SUPPRIMER') !== -1) {
                        $(this).addClass('color-SUPPRIMER');
                    }
                    if ($(this).text().indexOf('EXPORTE') !== -1) {
                        $(this).addClass('color-EXPORTE');
                    }
                    if ($(this).text().indexOf('VERIFIE') !== -1) {
                        $(this).addClass('color-VERIFIE');
                    }       
                });
                //updateSum();
            }
            $(".search-button").addClass("opacity-0");
            $(".generate").prop("disabled", false);
            
        }).fail(function (error) {
            // Handle error
            $(".search-button").addClass("opacity-0");
            $(".generate").prop("disabled", false);
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'Fermer',
                text: "Une erreur est survenue",
                footer: "S'il vous plaît essayer à nouveau"
            });
        })
            ;
    });
    function createAccordionItem(index, data) {
        const colorClasses = ['bg-info', 'bg-primary', 'bg-secondary', 'bg-success', 'bg-warning'];
        const randomColorClass = '' /*colorClasses[Math.floor(Math.random() * colorClasses.length)];*/
        var selectscannerid = $('#scanner' + data.enseigne);
        var enseigne = data.enseigne.replace(/\s/g, "");
        return $(`
        <div id="${data.enseigne}" class=" ${index === 0 ? `srounded-t-lg` : ` `}  border border-neutral-200">
            <h2 class="mb-0">
                <button class="group flex flex-wrap w-full items-center ${index === 0 ? `sbg-info srounded-t-[5px]` : randomColorClass }  border-0 px-4 py-3 text-left text-white-800 transition [overflow-anchor:none] hover:z-[2] focus:z-[3] focus:outline-none [&:not([data-te-collapse-collapsed])]:text-white [&:not([data-te-collapse-collapsed])]:[box-shadow:inset_0_-1px_0_rgba(229,231,235)]"
                    type="button"
                    data-te-collapse-init
                    data-te-target="${`#collapse` + enseigne}"
                    aria-expanded="true"
                    aria-controls="${`collapse` + enseigne}">
                    <h5 class="font-bold text-black tracking-wide w-[150px]">${data.enseigne}</h5>
                    <div style="font-size: 17px !important" class="ml-auto h-18 w-50 flex-wrap sm:flex justify-center gap-2 sm:w-50 overflow-auto">
                        <span style="width: 88px" title="Nombre de lots (jour)" class="inline-block whitespace-nowrap rounded-full bg-neutral-50 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-neutral-600"> ${data.lotjour} </span>
                        <span style="width: 70px" title="Nombre de lots" class="inline-block whitespace-nowrap rounded-full bg-neutral-50 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-neutral-600">
                            ${data.nbLotsglobal - data.nbLotdeleted}
                        </span>
                        <span style="width: 106px" title="Nombre de lots traités" class="inline-block whitespace-nowrap rounded-full bg-success-200 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-neutral-600">
                            ${data.nbLotTraite}
                        </span>
                        <span style="width: 70px" title="Nombre de plis" class="hidden sm:inline-block  whitespace-nowrap rounded-full bg-secondary-200 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-success-700">
                            ${data.nbplisglobal - data.nbPlisdeleted}
                        </span>
                        <span style="width: 108px" title="Nombre de plis non traités" class="hidden sm:inline-block whitespace-nowrap rounded-full bg-danger-200 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-warning-800">
                            ${data.nbPlinonTraites }
                        </span>
                        <span style="width: 90px" title="Nombre de plis traités" class="hidden sm:inline-block whitespace-nowrap rounded-full bg-success-200 px-[0.65em] pb-[0.30em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-neutral-600">
                            ${data.nbplisglobal - data.nbPlinonTraites - data.nbPliRejected}
                        </span>
                        <span style="width: 75px" title="Nombre de plis rejetés" class="hidden sm:inline-block whitespace-nowrap rounded-full bg-warning-200 px-[0.65em] pb-[0.30em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-danger-700">
                            ${data.nbPliRejected}
                        </span>
                        <span style="width: 110px" title="Nombre de lot supprimé" class="hidden sm:inline-block whitespace-nowrap rounded-full bg-danger-200 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-warning-800">
                            ${data.nbLotdeleted}
                        </span>
                        <span style="width: 64px" title="En cours de reconnaissance" class="inline-block whitespace-nowrap rounded-full bg-neutral-50 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-neutral-600">
                            ${data.nbLotsreco}
                        </span>
                    </div>
                    <span class="ml-auto h-5 w-5 shrink-0 transition-transform duration-200 ease-in-out group-[[data-te-collapse-collapsed]]:rotate-0 group-[[data-te-collapse-collapsed]]:fill-[#212529] motion-reduce:transition-none">
                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="h-5 w-5 font-bold">
                            <path stroke-linecap="round" stroke-linejoin="round" d="M19.5 8.25l-7.5 7.5-7.5-7.5" />
                        </svg>
                    </span>
                </button>
            </h2>
            <div id="${`collapse` + enseigne}" class="!visible hidden" data-te-collapse-item aria-labelledby="${enseigne}" data-te-parent="#accordion">
                <div class="px-5 py-4 bg-detail">
                    <div class="flex flex-col sm:flex-row gap-2 justify-between">
                        <div class="mb-3 w-full sm:w-64">
                            <select id="scanner${enseigne}" name="scanner${enseigne}" onchange="filterCheckboxScanner('${enseigne}')" class="p-2 shadow focus:outline-none block w-full sm:text-sm border-gray-500 rounded-md">
                                <option>Scanner</option>
                                ${(data.listscanner ?? []).map(item => `<option value="${item}">${item}</option>`).join('')}
                            </select>
                        </div>
                        <div class="w-full">
                            <div class="flex-wrap sm:flex justify-center items-center space-x-4">
                                <label for="checkboxEncours${enseigne}" class="flex items-center space-x-2">
                                    <input type="checkbox" id="checkboxEncours${enseigne}" checked class="form-checkbox text-indigo-600" onclick="filterCheckboxScanner('${enseigne}')">
                                        <span class="text-gray-700">En cours / IMP</span>
                                </label>

                                <label for="checkboxTraites${enseigne}" class="flex items-center space-x-2">
                                    <input type="checkbox" id="checkboxTraites${enseigne}" checked class="form-checkbox text-indigo-600" onclick="filterCheckboxScanner('${enseigne}')">
                                        <span class="text-gray-700">Traités</span>
                                </label>

                                <label for="checkboxsupprimes${enseigne}" class="flex items-center space-x-2">
                                    <input type="checkbox" id="checkboxsupprimes${enseigne}" checked class="form-checkbox text-indigo-600" onclick="filterCheckboxScanner('${enseigne}')">
                                        <span class="text-gray-700">Supprimés</span>
                                </label>

                                <label for="checkboxreco${enseigne}" class="flex items-center space-x-2">
                                    <input type="checkbox" id="checkboxreco${enseigne}" checked class="form-checkbox text-indigo-600" onclick="filterCheckboxScanner('${enseigne}')">
                                    <span class="text-gray-700">Réco</span>
                                </label>
                            </div>
                        </div>
                        <div class="mb-3 w-full sm:w-64">
                            <div class="relative rounded-md shadow">
                                <input type="text" id="search${enseigne}" onkeyup="filterCheckboxScanner('${enseigne}')" class="p-2 focus:outline-none block w-full sm:text-sm border-gray-500 rounded-md" placeholder="N° opex..." />
                                <div class="absolute inset-y-0 right-0 pr-3 flex items-center pointer-events-none">
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-5 h-5">
                                        <path stroke-linecap="round" stroke-linejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
                                    </svg>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="flex flex-col">
                        <div class="overflow-x-auto sm:-mx-2 lg:-mx-4">
                            <div class="inline-block min-w-full py-2 sm:px-2 lg:px-4">
                                <div class="overflow-hidden">
                                    <table class="min-w-full text-sm font-light">
                                        <thead class="border-b font-medium">
                                            <tr>
                                                <th scope="col" class="px-2 py-1">DATE</th>
                                                <th scope="col" class="px-2 py-1">LOTS</th>
                                                <th scope="col" class="px-2 py-1">N° OPEXS</th>
                                                <th scope="col" class="px-2 py-1 text-left">STATUT</th>
                                                <th scope="col" class="px-2 py-1 text-right">PLIS</th>
                                                <th scope="col" class="px-2 py-1 text-right">REJETS</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            ${(data.detailModel ?? []).map((item, index) => `
                                                <tr class="dt-row${enseigne} hover:bg-gray-400 transition-colors duration-300 border-b ${index % 2 !== 0 ? `bg-gray-200` : ``}">
                                                   <td class="whitespace-nowrap px-3 py-1 font-medium">${item.dateimport}</td>
                                                   <td class="whitespace-nowrap px-3 py-1">${item.lotid}</td>
                                                   <td class="whitespace-nowrap px-3 py-1 dt-opex${enseigne}">${item.nom_fichier}</td>
                                                   <td class="whitespace-nowrap px-3 py-1 text-left dt-statut${enseigne}">
                                                      ${(item.status === "SUPPRIMER") ? item.statut :
                                                      (item.statut === "EN COURS") ? item.statut + ` par ` + item.processedBy :
                                                      (item.statut === "EXPORTE") ? item.statut + `(` + item.processedBy + `)` :
                                                      (item.statut === "SAISI") ? item.statut + ` par ` + item.processedBy : (item.statut === "Dernier accès") ? `Dernier accès par ` + item.processedBy :
                                                      (item.statut === "IMP" || item.statut === "RECO") ? item.statut : (item.statut === "VERIFIE") ? item.statut + ` par ` + item.processedBy : item.statut}
                                                   </td>
                                                   <td class="whitespace-nowrap px-3 py-1 text-right">${item.nbPli}</td>
                                                   <td class="whitespace-nowrap px-3 py-1 text-right">${item.rejectedCount}</td>
                                                </tr>`
                                            ).join('')}
                                        </tbody>
                                    </table>
                                    <div id="${`noResults`+enseigne}" class="hidden p-4 text-center">Aucune donnée ne correspond.</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `);
    }

    //Datatable
    //var table = new DataTable('#datatableid', {
    //    columns: [
            
    //        {
    //            data: 'date', className: 'nowrap', width: '15%',
    //            render: function (data, type, row) {
    //                var dateParts = data.split('-');
    //                if (dateParts.length === 3) {
    //                    var year = dateParts[2];
    //                    var month = dateParts[1];
    //                    var day = dateParts[0];
    //                    return day + '/' + month + '/' + year;
    //                }
    //                return data;
    //            }
    //        },
    //        { data: 'enseigne', className: 'nowrap', width: '15%' },
    //        { data: 'nom_fichier', className: 'text-right' },
    //        { data: 'lotid', className: 'text-right' },
    //        { data: 'no_pli', className: 'text-right' },
    //        { data: 'rejectedCount', className: 'text-right', width: '15%' },
    //        //{ data: 'paiement', className: 'text-right' },
    //        {
    //            data: null,
    //            className: 'text-left', width: '15%',
    //            render: function (data, type, row) {
    //                const processedByname = row['opr'];
    //                const statut = row['impstatut'];
    //                let textStatut = '';
    //                let bgcolor = "F0000";
    //                textStatut = (status === "SUPPRIMER") ? `${statut}` : (statut === "EN COURS") ? `${statut} par ${processedByname}` : (statut === "EXPORTE") ? textStatut = `${statut} (${processedByname})` : (statut === "SAISI") ? `${statut} par ${processedByname}` : (statut === "Dernier accès") ? `Dernier accès par ${processedByname}` : (statut === "IMP" || statut === "RECO") ? `${statut}` : (statut === "VERIFIE") ? `${statut} par ${processedByname}` : `${statut}`;
    //                return `${textStatut}`;
    //            }
    //        },
           
    //    ],
    //    language: {
    //        emptyTable: "Aucune donnée disponible dans le tableau",
    //        loadingRecords: "Chargement...",
    //        processing: "Traitement...",
    //        search: "Filtrer:",
    //        info: "Affichage de _START_ à _END_ sur _TOTAL_ LOT(S)",
    //        infoEmpty: "Affichage de 0 à 0 sur 0 entrées",
    //        infoFiltered: "(filtrées depuis un total de _MAX_ entrées)",
    //        lengthMenu: "Afficher _MENU_ entrées",
    //        paginate: {
    //            first: "Première",
    //            last: "Dernière",
    //            next: "Suivante",
    //            previous: "Précédente"
    //        },
    //        zeroRecords: "Aucune entrée correspondante trouvée"
    //    },
    //    initComplete: function () {
    //        $('#datatableid_filter input[type="search"]').attr('id', 'customSearchInput');
    //    },
    //    order: [[2, 'asc']],
    //    rowId: 'id',
    //    stateSave: true,
    //    lengthMenu: [12, 30, 45, 65, 150],
    //    pageLength: 12
    //});

    //search
    //var customSearchInput = $('#customSearchInput')
    //customSearchInput.on('input', function () {
    //    var searchTerm = $(this).val();
    //    updateSum();
    //    table.search(searchTerm).draw();
    //    highlightSearchTerm(table, searchTerm);
    //});
    //function highlightSearchTerm(table, searchTerm) {
    //    table.cells().nodes().to$().removeClass('text-primary');
    //    if (searchTerm.trim() !== '') {
    //        var regex = new RegExp(searchTerm, 'i');
    //        table.cells().nodes().to$().filter(function () {
    //            return regex.test($(this).text());
    //        }).addClass('text-primary');
    //    }
    //}

    //updateSum();

    //// Écoute l'événement de recherche
    //table.on('search.dt', function () {
    //    // Mise à jour de la somme lors de la recherche
        
    //});

    //function updateSum() {
    //    // Calcule la somme de la colonne
    //    var sumPli = table.column(4, { search: 'applied' }).data().reduce(function (a, b) {
    //        return a + b;
    //    }, 0);
    //    var sumRejet = table.column(5, { search: 'applied' }).data().reduce(function (a, b) {
    //        return a + b;
    //    }, 0);
    //    $('#datatableid tfoot th:eq(4)').text(sumPli);
    //    $('#datatableid tfoot th:eq(5)').text(sumRejet);

    //}
    // END SEARCH

    var indicator = $('.toggle-indicator');
    var switchElement = $('.toggle-switch');
    var checkbox = $('#toggle');

    // Au chargement de la page
    if (checkbox.prop('checked')) {
        indicator.css({ left: '40%' });
        switchElement.addClass('checked');
    }

    // Lorsque la case à cocher change
    checkbox.change(function () {
        //table.clear().draw();
        $("#lotstraites").text(0);
        $("#plistraites").text(0);
        $('#accordion').children(':not(.noremove)').remove();
        if ($(this).prop('checked')) {
            indicator.animate({ left: '40%' }, 200);
            switchElement.addClass('checked');
        } else {
            indicator.animate({ left: 0 }, 200);
            switchElement.removeClass('checked');
        }
    });
    //Enseigne section
    $('.check-all').on('click', function () {
        //table.clear().draw();
        $("#lotstraites").text(0);
        $("#plistraites").text(0);
        $('#accordion').children(':not(.noremove)').remove();
        var checkboxes = $('input[type=checkbox].pcheck');
        $('.generate').prop('disabled', !$(this).prop('checked'));
        checkboxes.prop('checked', $(this).prop('checked'));
    });
    $("input[type='checkbox'].pcheck").change(function () {
        //table.clear().draw();
        $("#lotstraites").text(0);
        $("#plistraites").text(0);
        $('#accordion').children(':not(.noremove)').remove();
        var checkboxes = $('input[type=checkbox].pcheck');
        if (checkboxes.filter(":checked").length > 0) {
            $(".generate").prop("disabled", false);
        } else {
            $(".generate").prop("disabled", true);
        }
        if (checkboxes.length == checkboxes.filter(":checked").length) {
            $(".check-all").prop("checked", true);
            indicator.animate({ left: '40%' }, 200);
            switchElement.addClass('checked');
        } else {
            $(".check-all").prop("checked", false);
            indicator.animate({ left: 0 }, 200);
            switchElement.removeClass('checked');
        }
    });

})(jQuery)

// search script section
function filterCheckboxScanner(enseigne) {
    var checkboxTraites = $("#checkboxTraites" + enseigne);
    var checkboxsupprimes = $("#checkboxsupprimes" + enseigne);
    var checkboxEncours = $("#checkboxEncours" + enseigne);
    var checkboxreco = $("#checkboxreco" + enseigne);
    var opexText = $("#search" + enseigne).val().toLowerCase();
    var scannerValue = $("#scanner" + enseigne).val();
    var scanner = scannerValue !== null ? scannerValue.split(',') : [];
    var ListCheckbox = [];
    if (checkboxEncours.prop("checked")) { ListCheckbox.push("EN COURS"); ListCheckbox.push("Dernier"); ListCheckbox.push("IMP"); }
    if (checkboxTraites.prop("checked")) { ListCheckbox.push("EXPORTE"); ListCheckbox.push("SAISI"); }
    if (checkboxsupprimes.prop("checked")) { ListCheckbox.push("SUPPRIMER"); }
    if (checkboxreco.prop("checked")) { ListCheckbox.push("RECO"); }
    if (scanner.length > 0) ListCheckbox = ListCheckbox.concat(scanner);
    //console.log(ListCheckbox); 
    var anyMatch = false;
    $(".dt-row"+enseigne).each(function () {
        var statutColumn = $(this).find(".dt-statut"+enseigne).text().toLowerCase();
        var opexColumn = $(this).find(".dt-opex"+enseigne).text().toLowerCase();
        var twoChar = opexColumn.substring(0, 2);
        var statutMatch = ListCheckbox.some(function (checkboxItem) {
            return new RegExp(checkboxItem, 'i').test(statutColumn);
        });
        var twoCharMatch = scannerValue === "Scanner" || ListCheckbox.some(function (checkboxItem) {
            return new RegExp(checkboxItem, 'i').test(twoChar);
        });

        var opexTextMatch = opexText === "" || opexText === null ? true : opexText.includes(opexColumn);

        if (statutMatch && twoCharMatch && opexTextMatch) {
            $(this).show();
            anyMatch = true;
        } else {
            $(this).hide();
        }
    });

    if (!anyMatch) {
        $("#noResults" + enseigne).show();
    } else {
        $("#noResults" + enseigne).hide();
    }
};

///// end search script section
