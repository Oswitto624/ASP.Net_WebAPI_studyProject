Catalog = {
    _properties: {
        getViewLink: ""
    },

    init: properties => {
        $.extend(Catalog._properties, properties);

        $(".pagination li a").click(Catalog.clickOnPage);
    },

    clickOnPage: async function (e) {
        e.preventDefault();
        const button = $(this);

        if (button.parent().hasClass("active")) return;

        const container = $("#catalog-container");

        container.LoadingOverlay("show");

        const data = button.data();
        let query = "";
        for (let key in data)
            if (data.hasOwnProperty(key))
                query += `${key}=${data[key]}&`;

        const response = await fetch(`${Catalog._properties.getViewLink}?${query}`);

        if (response.ok) {
            container.html(await response.text());

            $(".pagination li").removeClass("active");
            button.parent().addClass("active");

        } else {
            console.log("clickOnPage fail", response.status);
        }

        container.LoadingOverlay("hide");
    }
}