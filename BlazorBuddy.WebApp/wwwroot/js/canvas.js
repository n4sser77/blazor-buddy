let isDrawing = false;
let canvas;
let ctx;
let lastX = 0;
let lastY = 0;
let currentMode = 'draw';
let fontSize = 16;
let textInputElement = null;
let onTextSubmitCallback = null;

export function initCanvas(canvasElement) {
    canvas = canvasElement;
    ctx = canvas.getContext('2d');
    

    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;

    ctx.strokeStyle = '#000000';
    ctx.lineWidth = 2;
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
    

    canvas.addEventListener('mousedown', handleMouseDown);
    canvas.addEventListener('mousemove', draw);
    canvas.addEventListener('mouseup', stopDrawing);
    canvas.addEventListener('mouseout', stopDrawing);
    

    canvas.addEventListener('touchstart', handleTouch);
    canvas.addEventListener('touchmove', handleTouch);
    canvas.addEventListener('touchend', stopDrawing);
}


function handleMouseDown(e) {
    if (currentMode === 'text') {
        const rect = canvas.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;
        showTextInput(x, y);
    } else {
        startDrawing(e);
    }
}

// Visa textinput på canvas
function showTextInput(x, y) {
    if (textInputElement) {
        textInputElement.remove();
    }
    
    // Skapa textinput-element
    textInputElement = document.createElement('input');
    textInputElement.type = 'text';
    textInputElement.style.position = 'fixed';
    textInputElement.style.left = (canvas.getBoundingClientRect().left + x) + 'px';
    textInputElement.style.top = (canvas.getBoundingClientRect().top + y) + 'px';
    textInputElement.style.fontSize = fontSize + 'px';
    textInputElement.style.fontFamily = 'Arial';
    textInputElement.style.border = '2px solid #000';
    textInputElement.style.padding = '4px';
    textInputElement.style.backgroundColor = 'white';
    textInputElement.style.zIndex = '10000';
    textInputElement.style.minWidth = '150px';
    
    document.body.appendChild(textInputElement);
    
    // Fokusera och välj texten
    setTimeout(() => {
        textInputElement.focus();
    }, 10);
    
    // Flagga för att vi är i textmode
    window.isTextInputActive = true;
    

    const handleKeyDown = (e) => {
        e.stopPropagation();
        if (e.key === 'Enter') {
            const text = textInputElement.value;
            if (text) {
                ctx.font = fontSize + 'px Arial';
                ctx.fillStyle = ctx.strokeStyle;
                ctx.fillText(text, x, y + fontSize);
            }
            cleanupTextInput();
        }
        if (e.key === 'Escape') {
            cleanupTextInput();
        }
    };
    
    textInputElement.addEventListener('keydown', handleKeyDown);

    const handleBlur = () => {
        setTimeout(() => {
            cleanupTextInput();
        }, 100);
    };
    
    textInputElement.addEventListener('blur', handleBlur);
}

// Rensa textinput
function cleanupTextInput() {
    if (textInputElement) {
        textInputElement.remove();
        textInputElement = null;
    }
    window.isTextInputActive = false;
    currentMode = 'draw';
    canvas.style.cursor = 'crosshair';
}

function startDrawing(e) {
    isDrawing = true;
    const rect = canvas.getBoundingClientRect();
    lastX = e.clientX - rect.left;
    lastY = e.clientY - rect.top;
}

function draw(e) {
    if (!isDrawing) return;
    
    const rect = canvas.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;

    ctx.beginPath();
    ctx.moveTo(lastX, lastY);
    ctx.lineTo(x, y);
    ctx.stroke();
    
    lastX = x;
    lastY = y;
}


function stopDrawing() {
    isDrawing = false;
}

function handleTouch(e) {
    e.preventDefault();
    const touch = e.touches[0];
    const mouseEvent = new MouseEvent(e.type === 'touchstart' ? 'mousedown' : 'mousemove', {
        clientX: touch.clientX,
        clientY: touch.clientY
    });
    canvas.dispatchEvent(mouseEvent);
}


export function setColor(color) {
    ctx.strokeStyle = color;
}

export function setLineWidth(width) {
    ctx.lineWidth = width;
}

export function setMode(mode) {
    currentMode = mode;
    if (mode === 'text') {
        canvas.style.cursor = 'text';
    } else {
        canvas.style.cursor = 'crosshair';
    }
}

export function setFontSize(size) {
    fontSize = size;
}



export function clearCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
}

export function getCanvasImage() {
    return canvas.toDataURL('image/png');
}

// Spara ritningen som JSON
export function saveDrawing() {
    const imageData = getCanvasImage();
    return {
        image: imageData,
        timestamp: new Date().toISOString()
    };
}

// Exportera currentMode för att hämta status
export function getCurrentMode() {
    return currentMode;
}
