import React from 'react';

type ComboboxInputProps = {
  getInputProps: () => React.InputHTMLAttributes<HTMLInputElement>;
  getToggleButtonProps: () => React.ButtonHTMLAttributes<HTMLButtonElement>;
  isOpen: boolean;
  placeholder: string;
};

export const ComboboxInput: React.FC<ComboboxInputProps> = ({
  getInputProps,
  getToggleButtonProps,
  isOpen,
  placeholder,
}) => (
  <div className="flex flex-col gap-1">
    <div className="flex shadow-sm bg-white gap-0.5">
      <input
        placeholder={placeholder}
        className="w-full p-1.5"
        {...getInputProps()}
      />
      <button
        aria-label="toggle menu"
        className="px-2"
        type="button"
        {...getToggleButtonProps()}
      >
        {isOpen ? <>&#8593;</> : <>&#8595;</>}
      </button>
    </div>
  </div>
);