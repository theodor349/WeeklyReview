import classNames from 'classnames';
import styles from './primary-button.module.scss';
import { Button } from "@/components/ui/button"


export interface PrimaryButtonProps {
    className?: string;
}

/**
 * This component was created using Codux's Default new component template.
 * To create custom component templates, see https://help.codux.com/kb/en/article/kb16522
 */
export const PrimaryButton = ({ className }: PrimaryButtonProps) => {
    return (
        <div>
          <Button>I am Primary Buttons</Button>
        </div>
      )
};
