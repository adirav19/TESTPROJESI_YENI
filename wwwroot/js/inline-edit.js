async function saveInlineEdit(cariKod, alan, yeniDeger, td) {
    const row = td.closest("tr");
    const dto = {
        CARI_KOD: cariKod,
        CARI_ISIM: row.querySelector(".cari-isim").textContent.trim(),
        CARI_TEL: row.querySelector(".cari-tel").textContent.trim(),
        CARI_IL: row.querySelector(".cari-il").textContent.trim(),
        EMAIL: ""
    };

    if (alan === "isim") dto.CARI_ISIM = yeniDeger;
    if (alan === "tel") dto.CARI_TEL = yeniDeger;
    if (alan === "il") dto.CARI_IL = yeniDeger;

    try {
        const response = await fetch("/TestApi/UpdateCari", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        if (response.ok) {
            td.textContent = yeniDeger;
            td.classList.add("bg-success", "text-white");
            setTimeout(() => td.classList.remove("bg-success", "text-white"), 1000);
        } else {
            td.textContent = "(Hata)";
        }
    } catch (err) {
        td.textContent = "(Hata)";
        console.error("Güncelleme hatası:", err);
    }
}