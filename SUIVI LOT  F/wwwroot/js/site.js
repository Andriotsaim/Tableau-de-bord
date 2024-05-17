// Write your JavaScript code.

(function ($) {
    'use strict';

    /////// Temps d'inactivité en millisecondes (30minutes 2s)
    const inactivityTimeout = 1802000; // 30minutes 2s
    let lastInteractionTime = Date.now();
    function checkInactivity() {
        const currentTime = Date.now();
        if (currentTime - lastInteractionTime >= inactivityTimeout) {
            location.reload();
        }
    }
    $(document).on("mousemove keydown", updateInteractionTime);

    function updateInteractionTime() {
        lastInteractionTime = Date.now();
    }
    setInterval(checkInactivity, 1000); // Check every second
    /////// end Temps d'inactivité en milliseconds

    /////// Event listeners menu
    const sidenav2 = document.getElementById("sidenav");
    const instance = te.Sidenav.getInstance(sidenav2);

    let innerWidth2 = null;

    const setMode2 = (e) => {
        // Check necessary for Android devices
        if (window.innerWidth === innerWidth2) {
            return;
        }

        innerWidth2 = window.innerWidth;
        if (window.innerWidth < instance.getBreakpoint("xl")) {
            instance.changeMode("over");
            instance.hide();
        } else {
            instance.changeMode("side");
            instance.show();
        }
    };
    if (window.innerWidth < instance.getBreakpoint("xl")) {
        setMode2();
    }

    window.addEventListener("resize", setMode2);
    let over = false;
    $("#slim-toggler").on("click", () => {
        if (window.innerWidth < instance.getBreakpoint("xl")) {
            instance.changeMode("over");
            instance.toggle();
        } else {
            if (!over) { over = true; instance.changeMode("over"); instance.hide(); }
            else { over = false; instance.changeMode("side"); instance.show(); }
        }

    });

    $("#sidenav").on("mouseover", function () {
        if (over) {
            const instance = te.Sidenav.getInstance($("#sidenav")[0]);
            instance.show();
        }
    });
    $("#sidenav").on("mouseout", function () {
        if (over) {
            const instance = te.Sidenav.getInstance($("#sidenav")[0]);
            instance.hide();
        }

    });
    /////// end Event listeners menu
    /////// logout
    $('#logoutLink').click(function () {
        Swal.fire({
            title: 'Déconnexion',
            text: "Êtes-vous sûr de vouloir vous déconnecter ?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Confirmer',
            cancelButtonText: 'Annuler'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = "/Account/Logout";
            }
        });
    });

    /////// End logout
    $('.tooltip').hover(function () {
        $(this).parent('.tooltip-container').find(':before').css('opacity', '1');
    }, function () {
        $(this).parent('.tooltip-container').find(':before').css('opacity', '0');
    });
})(jQuery);



