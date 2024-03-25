import { createBoard } from '@wixc3/react-board';
import { PrimaryButton } from '../../../components/primary-button/primary-button';

export default createBoard({
    name: 'PrimaryButton',
    Board: () => <PrimaryButton />,
    isSnippet: true,
});