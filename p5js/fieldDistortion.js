const canvasSize = 400;
const step = 50;
const points = [];
const directionVectors = [];
let arrowColor = "red";
let center;

function setup() {
	createCanvas(canvasSize, canvasSize);
    center = createVector(canvasSize / 2, canvasSize / 2);
    createGrid();
    createDirectionVectors(sourceDistortion);
    // createDirectionVectors(sinkDistortion);
}

function draw() {
	background(220);
    drawField();
}

function drawField()
{
    for(let i = 0; i < points.length; i++)
    {
        drawArrow(points[i], directionVectors[i], arrowColor);       
    }
}

function drawArrow(base, vec, myColor) {
	push();
	stroke(myColor);
	strokeWeight(3);
	fill(myColor);
	translate(base.x, base.y);
	line(0, 0, vec.x, vec.y);
	rotate(vec.heading());
	let arrowSize = 7;
	translate(vec.mag() - arrowSize, 0);
	triangle(0, arrowSize / 2, 0, -arrowSize / 2, arrowSize, 0);
	pop();
}

function createGrid(){
	for(let i = step/2; i < canvasSize; i+= step)
	{
		for(let j = step/2; j < canvasSize; j+= step)
		{
            if(i == center.x && j == center.y)
            {
                continue;
            }
			points.push(createVector(i, j));
		}
	}
}

function sourceDistortion(vector)
{
    arrowColor = "red";
    vector.normalize().mult(-step / 2);
}

function sinkDistortion(vector)
{
    arrowColor = "green";
    vector.normalize().mult(step / 2);
}

function createDirectionVectors(distortVector)
{
    for(let i = 0; i < points.length; i++)
    {
        let x = center.x - points[i].x;
        let y = center.y - points[i].y;
        let directionVector = createVector(x, y);
        if(distortVector)
        {
            distortVector(directionVector);
        }
        directionVectors.push(directionVector);
    }
}
