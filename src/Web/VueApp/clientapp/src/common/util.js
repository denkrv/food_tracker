/**
 * 
 * @param {Object} facts
 * {protein, carbohydrates, fat}
 * @returns {Array} 
 *  array of 3 elements. [calories from protein, calories from carb, calories from fat]
 */
export function genCaloriesSeq(facts) {
    return [(facts.protein || 0) * 4, (facts.carbohydrates || 0) * 4, (facts.fat || 0)* 9 ];
}

export function calcCalories(facts) {
    return (facts.protein || 0) * 4 + (facts.carbohydrates || 0) * 4 + (facts.fat || 0) * 9;
}
/**
 * 
 * @param {Number} calories
 * @param {Number} pp - protein percent in range 0..99
 * @param {Number} cp - carbohydrates percent in range 0..99
 * @param {Number} fp - fat percent in range 0..99
 * @returns {Object}
 *  nutrition facts  {protein, carbohydrates, fat, calories}
 */
export function genFacts(calories, pp, cp, fp) {
    return {
        protein: Math.round(calories * pp / 100 / 4),
        carbohydrates: Math.round(calories * cp / 100 / 4),
        fat: Math.round(calories * fp / 100 / 9),
        calories : calories
    };
}