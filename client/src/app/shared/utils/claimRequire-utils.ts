export const claimRequired = {
    adminOnly: (c: any) => c.role == 'Admin',
    adminOrTeacher: (c: any) => c.role == 'Admin' || c.role == 'Teacher',
    hasLibraryId: (c: any) => 'libraryId' in c,
    under10Only: (c: any) => getAgeFromDOB(c.dob) < 10,
    applyForMaternityLeave: (c: any) => c.gender == 'Female'
}

// Utility function to calculate age in years from a DOB string (DD-MM-YYYY)
export function getAgeFromDOB(dob: string): number {
    const [day, month, year] = dob.split('-').map(Number);
    const birthDate = new Date(year, month - 1, day);
    const today = new Date();

    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();

    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    return age;
}
