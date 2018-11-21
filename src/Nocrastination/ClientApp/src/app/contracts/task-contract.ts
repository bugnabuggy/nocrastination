export interface TaskContract {
    id?: string;
    name: string;
    startDate?: Date;
    endDate?: Date;
    isFinished?: boolean;
}
