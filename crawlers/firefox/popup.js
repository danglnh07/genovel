async function copyNovelContent(e) {
    e.preventDefault();
    const message = document.getElementById("message");

    const tabs = await browser.tabs.query({ active: true, currentWindow: true });
    const tab = tabs[0];

    let response;
    try {
        response = await browser.tabs.sendMessage(tab.id, { action: "extract" });
    } catch (err) {
        message.innerText = "Error: Could not connect to page. Try refreshing it.";
        return;
    }

    if (!response?.text) {
        message.innerText = "Could not find novel content on this page.";
        return;
    }

    navigator.clipboard.writeText(response.text).then(() => {
        message.innerText = "Novel content copied to clipboard!";
    }).catch(err => {
        message.innerText = "Failed to copy: " + err.message;
    });
}

document.querySelector("button").addEventListener("click", copyNovelContent);
document.getElementById("close").addEventListener("click", () => window.close());
