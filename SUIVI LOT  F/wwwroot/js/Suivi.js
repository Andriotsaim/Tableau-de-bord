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
            url: "/Performance/Suivi",
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
                if (response.typesearch === "enseigne") {
                    $(".hideop").removeClass("hidden");
                    $.each(response.data, function (index, data) {
                        var accordionItem = createAccordionItem(index, data);
                        $('#accordion').append(accordionItem);
                        lotstraites = lotstraites + data.countLotglobal;
                        plistraites = plistraites + data.countplisglobal;
                    });
                    $("#lotstraites").text(lotstraites);
                    $("#plistraites").text(plistraites);
                } else {
                    $(".hideop").addClass("hidden");
                    var accordionItem = listOperateurOnly(response.data);
                    $('#accordion').append(accordionItem);
                    //lotstraites = lotstraites + data.countLotglobal;
                    //plistraites = plistraites + data.countplisglobal;
                    //$("#lotstraites").text(lotstraites);
                    //$("#plistraites").text(plistraites);
                }
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
    function createAccordionItem(indexs, data) {
        const colorClasses = ['bg-info', 'bg-primary', 'bg-secondary', 'bg-success', 'bg-warning'];
        const randomColorClass = colorClasses[Math.floor(Math.random() * colorClasses.length)];
        var validMultipleTimes = JSON.stringify(data.validMultipleTimes);
        var enseigne = data.enseigne.replace(/\s/g, "");
        $("#lot").text(data.countvalidMultipleTimes);
        return $(`
                    <div id="${enseigne}" class=" ${indexs === 0 ? `rounded-t-lg` : ` `} border border-neutral-200>
                        <h2 class="mb-0">
                            <button class="group flex flex-wrap w-full  items-center rounded-t-[5px] border-0 px-4 py-3 text-left text-white-800 transition [overflow-anchor:none] hover:z-[2] focus:z-[3] focus:outline-none  [&:not([data-te-collapse-collapsed])]:text-white [&:not([data-te-collapse-collapsed])]:[box-shadow:inset_0_-1px_0_rgba(229,231,235)]"
                                    type="button"
                                    data-te-collapse-init
                                    data-te-target="${`#collapse` + enseigne}"
                                    aria-expanded="true"
                                    aria-controls="${`collapse` + enseigne}">
                                    <h5 class="font-bold text-gray-900 tracking-wide w-[150px]">${data.enseigne}</h5>
                                    <div class="ml-auto h-18 w-50 flex-wrap sm:flex justify-center gap-2 sm:w-50 overflow-auto">
                                    <span style="width: 90px" title="Nombre de lots traités" class="tooltip inline-block whitespace-nowrap rounded-full  bg-neutral-50 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-neutral-600">
                                        LOTS: ${data.countLotglobal}
                                    </span>
                                    <span style="width: 90px" title="Nombre de plis traités" class="tooltip whitespace-nowrap rounded-full bg-success-100 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-success-700">
                                        PLIS: ${data.countplisglobal}
                                    </span>
                                    <span style="width: 106px" title="Nombre de documents différents traités" class="tooltip hidden sm:inline-block whitespace-nowrap rounded-full bg-danger-100 px-[0.65em] pb-[0.30em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-danger-700">
                                        DOCDISTS: ${data.countdocdistglobal}
                                    </span>
                                    <span title="Date d'ouverture du premier pli" class="tooltip hidden sm:inline-block whitespace-nowrap rounded-full bg-warning-100 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-warning-800">
                                        DATE DEBUT: ${data.startdate.replace("/", "-")}
                                    </span>
                                    <span title="Date de validation du dernier pli" class="tooltip hidden sm:inline-block whitespace-nowrap rounded-full bg-warning-100 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-warning-800">
                                        DATE FIN: ${data.enddate.replace("/", "-")}
                                    </span>
                                    <span title="Durée total sans prendre en compte les heures libres" class="tooltip whitespace-nowrap rounded-full bg-warning-100 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-warning-800">
                                        DURÉE: ${data.durationglobal}
                                    </span>
                                </div>
                                <span class="ml-auto h-5 w-5 shrink-0 rotate-[-180deg] fill-[#336dec] transition-transform duration-200 ease-in-out group-[[data-te-collapse-collapsed]]:rotate-0 group-[[data-te-collapse-collapsed]]:fill-[#212529] motion-reduce:transition-none">
                                    <svg xmlns="http://www.w3.org/2000/svg"  fill="none"  viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="h-5 w-5 font-bold">
                                        <path stroke-linecap="round" stroke-linejoin="round" d="M19.5 8.25l-7.5 7.5-7.5-7.5" />
                                    </svg>
                                </span>
                            </button>
                        </h2>
                        <div id="${`collapse` + enseigne}" class="!visible hidden" data-te-collapse-item aria-labelledby="${enseigne}" data-te-parent="#accordion">
                            <div class="px-5 py-4 bg-detail"
                                <div class="flex flex-col sm:flex-row gap-2 justify-between">
                                    ${data.countvalidMultipleTimes != 0 ? 
                                    `<p class="text-base">
                                        <span class="mr-2"><span class="rounded-full bg-danger-100 px-[0.65em] pb-[0.30em] pt-[0.35em]">${data.countvalidMultipleTimes}</span> lot(s) traité(s) par plusieurs Opérateurs</span>
                                        <button type="button" id="modal-open" onclick='Detailmultiplevalidatepli(${validMultipleTimes})' data-fackname="item.Fackname" class="historique inline-flex gap-2 rounded-full bg-info-50 px-6 pb-1 pt-1 text-sm font-bold leading-normal text-primary shadow-[0_4px_9px_-4px_#cbcbcb] transition duration-150 ease-in-out hover:bg-neutral-100 hover:shadow-[0_8px_9px_-4px_rgba(203,203,203,0.3),0_4px_18px_0_rgba(203,203,203,0.2)] focus:bg-neutral-100 focus:shadow-[0_8px_9px_-4px_rgba(203,203,203,0.3),0_4px_18px_0_rgba(203,203,203,0.2)] focus:outline-none focus:ring-0 active:bg-neutral-200 active:shadow-[0_8px_9px_-4px_rgba(203,203,203,0.3),0_4px_18px_0_rgba(203,203,203,0.2)]">
                                            <span>Voir</span>
                                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="w-5 h-5 mt-[3px] rotate-[90deg]">
                                                <path fill-rule="evenodd" d="M2.25 4.5A.75.75 0 0 1 3 3.75h14.25a.75.75 0 0 1 0 1.5H3a.75.75 0 0 1-.75-.75Zm14.47 3.97a.75.75 0 0 1 1.06 0l3.75 3.75a.75.75 0 1 1-1.06 1.06L18 10.81V21a.75.75 0 0 1-1.5 0V10.81l-2.47 2.47a.75.75 0 1 1-1.06-1.06l3.75-3.75ZM2.25 9A.75.75 0 0 1 3 8.25h9.75a.75.75 0 0 1 0 1.5H3A.75.75 0 0 1 2.25 9Zm0 4.5a.75.75 0 0 1 .75-.75h5.25a.75.75 0 0 1 0 1.5H3a.75.75 0 0 1-.75-.75Z" clip-rule="evenodd" />
                                            </svg>
                                        </button>
                                    </p >`: ``}
                                </div>
                                <div class="flex flex-col">
                                    <div class="overflow-x-auto sm:-mx-2 lg:-mx-4">
                                        <div class="inline-block min-w-full py-2 sm:px-2 lg:px-4">
                                            <div class="overflow-hidden" style="margin: 0px 6px 0px 6px;>
                                                <div id="accordionchild">
                                                ${
                                                    (data.details ?? []).map((item, index) => `
                                                              ${listOperateur(item, index, enseigne)}                          
                                                    `).join('')
                                                }   
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
        `);
    }
    //$('#modal-open').click(function () {
    //    $('#modal').removeClass('hidden').hide().fadeIn('slow');
    //    $('body').addClass('modal-open');
    //});
    //$('#modal-close').click(function () {
    //    $('#modal').fadeOut('slow', function () {
    //        $('#modal').addClass('hidden');
    //    });
    //    $('body').removeClass('modal-open');
    //});
    

})(jQuery)

function Detailmultiplevalidatepli(detail) {
    //console.log(detail);
    $('#modal').removeClass('hidden').hide().fadeIn('normal');
    $('body').addClass('modal-open');
    $('.modal-close').click(function () {
        var modal = $('#modal');
        modal.fadeOut('normal', function () {
            modal.addClass('hidden');
        });
        $('body').removeClass('modal-open');
    });
    $('#tbodymultiplvalide').children(':not(.noremove)').remove();
    var multiplvldItem = `${(detail ?? []).map((item, index) => `
        ${(item ?? []).map((subparentItem) => `
            ${(subparentItem.details ?? []).map((sub, subindex) => `
                ${listduplicate(sub, subindex, index)}   
            `).join('')}
            `).join('')}
        `).join('')}`;
    $('#tbodymultiplvalide').append(multiplvldItem);
    //alert("OK");

}
var plot = "";
var oldop = "";
function listduplicate(data, subindex, subparentItemindex) {
    var positionlotempty = plot + oldop === data.lotid + data.operateur ? "." : data.lotid;
    if (subindex === 0) {
        plot = data.lotid; positionlotempty = data.lotid; oldop = data.operateur
    }
    if (positionlotempty != ".") { plot = data.lotid; oldop = data.operateur; }
    return `<tr class="dt-row ${subparentItemindex % 2 === 0 ? "bg-slate-200" : "" } hover:bg-gray-400 transition-colors duration-300 border-b">
            <td class="whitespace-nowrap px-3 py-1 border-gray-300 border-solid border-b-2">
                <div class="text-sm text-gray-900">${positionlotempty}</div>
            </td>
            <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                <div class="px-3 text-sm text-left">${positionlotempty === "." ? "." : data.operateur}</div>
            </td>
            <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                <div class="px-3 text-sm text-left">${data.pli}</div>
            </td>
            <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                <div class="px-3 text-sm text-left">${data.docid}</div>
            </td>
            <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                <div class="px-3 text-sm">${data.startdate}</div>
            </td>
            <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                <div class="px-3 text-sm">${data.enddate}</div>
            </td>
            <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                <div class="px-3 text-sm">${data.duration}</div>
            </td>
        </tr>`;
}
function listOperateur(item, index, enseigne ) {
    let colorClasses = ['bg-info', 'bg-primary', 'bg-secondary', 'bg-success', 'bg-warning'];
    let randomColorClass = colorClasses[Math.floor(Math.random() * colorClasses.length)];
    return `
        
        <div id="${enseigne + item.operateur}" border border-neutral-200">
            <h2 class="mb-0">
                <button class="group flex flex-wrap w-full items-center  ${index === 0 ? `bg-info` : randomColorClass } border-0 px-4 py-3 text-left text-white-800 transition [overflow-anchor:none] hover:z-[2] focus:z-[3] focus:outline-none dark:bg-neutral-800 dark:text-white [&:not([data-te-collapse-collapsed])]:text-white [&:not([data-te-collapse-collapsed])]:[box-shadow:inset_0_-1px_0_rgba(229,231,235)]" type="button"
                        data-te-collapse-init
                        data-te-target="${'#collapsechild' + enseigne + item.operateur}"
                        aria-expanded="true"
                        aria-controls="${'collapsechild' + enseigne + item.operateur}">
                        <h5 class="font-bold text-white tracking-wide w-[150px]">${item.operateur}</h5>
                        <div class="ml-auto h-18 w-50 flex-wrap sm:flex justify-center gap-2 sm:w-50 overflow-auto">
                            <span style="width: 90px" title="Nombre de lots traités par l'opérateur" class="tooltip inline-block whitespace-nowrap rounded-full bg-neutral-50 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-neutral-600">
                                LOTS: ${item.countLots}
                            </span>
                            <span style="width: 90px" title="Nombre de plis traités par l'opérateur" class="tooltip whitespace-nowrap rounded-full bg-success-100 mx-1 sm:mx-0 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-success-700">
                                PLIS: ${item.countplis}
                            </span>
                            <span style="width: 106px" title="Nombre de documents différents traités par l'opérateur" class="tooltip whitespace-nowrap rounded-full bg-danger-100 mx-1 sm:mx-0 px-[0.65em] pb-[0.30em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-danger-700">
                                DOCDISTS: ${item.countDocdist}
                            </span>
                            <span title="Ouverture du premier pli par l'opérateur" class="tooltip hidden sm:inline-block whitespace-nowrap rounded-full bg-warning-100 mx-1 sm:mx-0 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-warning-800">
                                DATE DEBUT: ${item.startdate}
                            </span>
                            <span title="Fermeture du dernier pli par l'opérateur" class="tooltip hidden sm:inline-block whitespace-nowrap rounded-full bg-warning-100 mx-1 sm:mx-0 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-warning-800">
                                DATE FIN: ${item.enddate}
                            </span>
                            <span title="Durée totale sans prendre en compte les heures libres" class="tooltip whitespace-nowrap rounded-full bg-warning-100 mx-1 sm:mx-0 px-[0.65em] pb-[0.25em] pt-[0.35em] text-center align-baseline text-[0.75em] font-bold leading-none text-warning-800">
                                DURÉE: ${item.duration}
                            </span>
                        </div>
                    <span class="ml-auto h-5 w-5 shrink-0 rotate-[-180deg] fill-[#336dec] transition-transform duration-200 ease-in-out group-[[data-te-collapse-collapsed]]:rotate-0 group-[[data-te-collapse-collapsed]]:fill-[#212529] motion-reduce:transition-none">
                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="h-5 w-5 font-bold">
                            <path stroke-linecap="round" stroke-linejoin="round" d="M19.5 8.25l-7.5 7.5-7.5-7.5" />
                        </svg>
                    </span>
                </button>
            </h2>
            <div id="${'collapsechild' + enseigne + item.operateur}" class="!visible hidden" data-te-collapse-item aria-labelledby="${enseigne + item.operateur}" data-te-parent="#accordionchild">
                <div class="px-5 py-4 bg-detail">
                    <div class="flex flex-col">
                        <div class="overflow-x-auto sm:-mx-2 lg:-mx-4">
                            <div class="inline-block min-w-full py-2 sm:px-2 lg:px-4">
                                <div class="overflow-hidden">
                                    <table class="min-w-full text-sm font-light">
                                        <thead class="border-b font-medium">
                                            <tr>
                                                <th scope="col" class="px-2 py-1">LOTS</th>
                                                <th scope="col" class="px-2 py-1 text-left">PLIS</th>
                                                <th scope="col" class="px-2 py-1 text-left">DOC DISTINCT</th>
                                                <th scope="col" class="px-2 py-1">DATE DEBUT(Ouverture doc)</th>
                                                <th scope="col" class="px-2 py-1">DATE FIN(Cloture doc)</th>
                                                <th title="Durée sans prendre en compte les heures libres" scope="col">DURÉE</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            ${
                                                (item.detailspli ?? []).map((item2, index2) => `
                                                            ${listDetailpli(item2, index2, enseigne, item.operateur)}                          
                                                `).join('')
                                            }    
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        `;
}

function listOperateurOnly(data) {
    var a = "test";
    return `
        <div class="px-5 py-4 bg-detail" >
            <div class="flex flex-col">
                <div class="overflow-x-auto sm:-mx-2 lg:-mx-4">
                    <div class="inline-block min-w-full py-2 sm:px-2 lg:px-4">
                        <div class="overflow-hidden">
                            <table class="min-w-full text-sm font-light">
                                <thead class="border-b font-medium">
                                    <tr>
                                        <th scope="col" class="px-2 py-1">OPERATEUR</th>
                                        <th scope="col" class="px-2 py-1 text-left">LOTS</th>
                                        <th scope="col" class="px-2 py-1 text-left">PLIS</th>
                                        <th scope="col" class="px-2 py-1 text-left">DOC DISTINCT</th>
                                        <th scope="col" class="px-2 py-1">DATE DEBUT(Ouverture doc)</th>
                                        <th scope="col" class="px-2 py-1">DATE FIN(Cloture doc)</th>
                                        <th title="Durée sans prendre en compte les heures libres" scope="col">DURÉE</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    ${
                                        (data ?? []).map((item, index) => `
                                                <tr class="dt-row${item.operateur} hover:bg-gray-400 transition-colors duration-300 border-b ${index % 2 !== 0 ? `bg-gray-200` : ``}">
                                                <td class="whitespace-nowrap py-1  border-gray-300  border-solid border-b-2">
                                                        <div class="text-sm pl-1 text-gray-900 text-left">${item.operateur}</div>
                                                    </td>    
                                                <td class="whitespace-nowrap py-1  border-gray-300  border-solid border-b-2">
                                                        <div class="text-sm text-gray-900 text-left">${item.countLots}</div>
                                                    </td>
                                                    <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                                                        <div class="px-3 text-sm text-left">${item.countplis}</div>
                                                    </td>
                                                    <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                                                        <div class="px-3 text-sm text-left">${item.countDocdist}</div>
                                                    </td>
                                                    <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                                                        <div class="px-3 text-sm">${item.startdate}</div>
                                                    </td>
                                                    <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                                                        <div class="px-3 text-sm">${item.enddate}</div>
                                                    </td>
                                                    <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
                                                        <div class="px-3 text-sm">${item.duration}</div>
                                                    </td>
                                                </tr>                         
                                        `).join('')
                                    }
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>`
}

var positionlot = "";
var oldopr = "";
function listDetailpli(item2, index, enseigne, operateur) {
    var positionlotempty = positionlot + oldopr === item2.lotid + operateur ? "." : item2.lotid;
    if (index === 0) {
        positionlot = item2.lotid; positionlotempty = item2.lotid; oldopr = operateur
    }
    if (positionlotempty != ".") { positionlot = item2.lotid; oldopr = operateur; }
    return `
    <tr class="dt-row${enseigne + operateur} hover:bg-gray-400 transition-colors duration-300 border-b ${index % 2 !== 0 ? `bg-gray-200` : ``}">
        <td class="whitespace-nowrap py-1  border-gray-300  border-solid border-b-2">
            <div class="text-sm text-gray-900">${positionlotempty != "." ? positionlotempty + ` ( ` + item2.durationlot + ` ) ` : positionlotempty}</div>
        </td>
        <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
            <div class="px-3 text-sm text-left">${item2.pli}</div>
        </td>
        <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
            <div class="px-3 text-sm text-left">${item2.countDocdist}</div>
        </td>
        <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
            <div class="px-3 text-sm">${item2.startdate}</div>
        </td>
        <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
            <div class="px-3 text-sm">${item2.enddate}</div>
        </td>
        <td class="whitespace-nowrap py-1 border-gray-300 border-solid border-b-2">
            <div class="px-3 text-sm">${item2.duration}</div>
        </td>
    </tr>
    `;
}
