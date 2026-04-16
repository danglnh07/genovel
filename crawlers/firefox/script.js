browser.runtime.onMessage.addListener((message, _sender, sendResponse) => {
    if (message.action !== "extract") return;

    let container = document.getElementById("novel_content");
    if (!container) {
        sendResponse({ text: null });
        return;
    }

    let content = container.children[1];
    let text = "Translate this chapter fully without omitting any parts: \n";
    for (let paragraph of content.children) {
        if (paragraph.tagName === "P") {
            text += paragraph.innerText + "\n\n";
        } else if (paragraph.tagName === "BR") {
            text += "\n\n";
        }
    }

    sendResponse({ text: text.trim() });
});
