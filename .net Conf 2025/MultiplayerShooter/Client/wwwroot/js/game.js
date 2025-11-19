// Game rendering and input handling via JavaScript Interop

let canvasContext = null;
let dotNetHelper = null;

window.isMobileDevice = () => {
    return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
};

window.initializeCanvas = (canvas) => {
    canvasContext = canvas.getContext('2d');
};

window.setupKeyboardInput = (helper) => {
    dotNetHelper = helper;

    document.addEventListener('keydown', (e) => {
        if (dotNetHelper) {
            dotNetHelper.invokeMethodAsync('OnKeyDown', e.key);
        }
    });

    document.addEventListener('keyup', (e) => {
        if (dotNetHelper) {
            dotNetHelper.invokeMethodAsync('OnKeyUp', e.key);
        }
    });
};

window.clearCanvas = (canvas) => {
    const ctx = canvas.getContext('2d');
    ctx.fillStyle = '#1a1a2e';
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // Draw grid
    ctx.strokeStyle = 'rgba(255, 255, 255, 0.05)';
    ctx.lineWidth = 1;
    for (let x = 0; x < canvas.width; x += 50) {
        ctx.beginPath();
        ctx.moveTo(x, 0);
        ctx.lineTo(x, canvas.height);
        ctx.stroke();
    }
    for (let y = 0; y < canvas.height; y += 50) {
        ctx.beginPath();
        ctx.moveTo(0, y);
        ctx.lineTo(canvas.width, y);
        ctx.stroke();
    }
};

window.drawPlayer = (canvas, x, y, color, angle, isAlive) => {
    const ctx = canvas.getContext('2d');

    if (!isAlive) {
        ctx.globalAlpha = 0.3;
    }

    ctx.save();
    ctx.translate(x, y);
    ctx.rotate(angle);

    // Draw ship body
    ctx.fillStyle = color;
    ctx.beginPath();
    ctx.moveTo(15, 0);
    ctx.lineTo(-10, -10);
    ctx.lineTo(-10, 10);
    ctx.closePath();
    ctx.fill();

    // Draw ship outline
    ctx.strokeStyle = 'white';
    ctx.lineWidth = 2;
    ctx.stroke();

    // Draw thruster
    ctx.fillStyle = '#ff6b6b';
    ctx.fillRect(-10, -3, -5, 6);

    ctx.restore();
    ctx.globalAlpha = 1;
};

window.drawBullet = (canvas, x, y) => {
    const ctx = canvas.getContext('2d');

    ctx.fillStyle = '#ffff00';
    ctx.beginPath();
    ctx.arc(x, y, 3, 0, Math.PI * 2);
    ctx.fill();

    // Glow effect
    ctx.fillStyle = 'rgba(255, 255, 0, 0.3)';
    ctx.beginPath();
    ctx.arc(x, y, 6, 0, Math.PI * 2);
    ctx.fill();
};

window.drawText = (canvas, text, x, y, color) => {
    const ctx = canvas.getContext('2d');

    ctx.font = '14px Arial';
    ctx.fillStyle = color;
    ctx.textAlign = 'center';
    ctx.fillText(text, x, y);
};

window.getCanvasMousePosition = (canvas, event) => {
    const rect = canvas.getBoundingClientRect();
    return {
        x: event.clientX - rect.left,
        y: event.clientY - rect.top
    };
};
