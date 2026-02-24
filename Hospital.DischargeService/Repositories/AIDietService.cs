namespace Hospital.DischargeService.Repositories
{
    public class AIDietService : IAIDietService
    {
        public Task<string> GenerateDietAsync(string diagnosis, int patientAge)
        {
            var d = diagnosis.ToLowerInvariant();
            string diet;

            if (d.Contains("diabetes") || d.Contains("diabetic"))
                diet = "Sugar-controlled diet: low glycaemic index foods, high fibre vegetables (broccoli, spinach), lean proteins, avoid refined carbohydrates and sugary drinks.";

            else if (d.Contains("hypertension") || d.Contains("cardiac") || d.Contains("heart"))
                diet = "Cardiac/Low-sodium diet: DASH diet plan, limit sodium to <2g/day, potassium-rich foods (bananas, sweet potatoes), avoid processed foods and saturated fats.";

            else if (d.Contains("kidney") || d.Contains("renal"))
                diet = "Renal diet: limit potassium, phosphorus and protein. Avoid high-potassium fruits. Consult a renal dietician for personalised targets.";
            
            else if (d.Contains("liver") || d.Contains("hepat"))
                diet = "Liver-friendly diet: low fat, adequate protein (0.8–1g/kg), avoid alcohol completely, favour complex carbohydrates and fresh vegetables.";
            
            else if (d.Contains("anaemia") || d.Contains("anemia"))
                diet = "Iron-rich diet: leafy greens, red meat (moderate), legumes, vitamin C alongside iron-rich foods to enhance absorption.";
            
            else if (patientAge < 18)
                diet = "Paediatric high-protein diet: dairy, eggs, lean meats, legumes — supporting growth and recovery. Avoid processed snacks.";
            
            else if (patientAge >= 65)
                diet = "Senior nutrition plan: calcium and vitamin D rich foods, adequate protein to prevent muscle loss, stay well-hydrated, soft-textured options if needed.";
            
            else
                diet = "Balanced recovery diet: adequate protein (1g/kg body weight), colourful vegetables, whole grains, fruits, 2–2.5L water daily. Avoid alcohol and smoking.";

            return Task.FromResult(diet);
        }
    }
}
